using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using Bogus;
using Bogus.DataSets;

namespace DesktopApp
{
    public class SignalRService : ISignalRService
    {
        private readonly object _lock = new object();
        private ImmutableArray<PatientVaccineInfo> _data;
        private readonly Lazy<IObservable<ImmutableArray<PatientVaccineInfo>>> _lazyObs;

        public SignalRService()
        {
            _data = Enumerable.Range(0, 2).Select(_CreateInfo).ToImmutableArray();
            _lazyObs = new Lazy<IObservable<ImmutableArray<PatientVaccineInfo>>>(() => Observable.Interval(TimeSpan.FromSeconds(10)).Select(
                _ =>
                {
                    lock (_lock)
                    {
                        _data = _Update(_data);
                    }
                    return _data;
                }));
        }
        
        public IObservable<ImmutableArray<PatientVaccineInfo>> GetObservablePatientVaccineInfo() => _lazyObs.Value;

        private static ImmutableArray<PatientVaccineInfo> _Update(ImmutableArray<PatientVaccineInfo> existing)
        {
            var randomInt = new Random().Next();
            var updated = existing.Select(x => _RandomUpdate(x, randomInt)).ToList();
            updated.Add(_CreateInfo(existing.Length));
            return updated.ToImmutableArray();
        }

        private static PatientVaccineInfo _RandomUpdate(PatientVaccineInfo existing, int randomInt) =>
            existing.Status == "Booked" || randomInt == 0
                ? existing
                : _UpdateStatus(existing, "Booked");

        private static PatientVaccineInfo _UpdateStatus(PatientVaccineInfo existing, string status) =>
            new PatientVaccineInfo(
                existing.PatientId,
                existing.NhsNumber,
                existing.FirstName,
                existing.LastName,
                status: status,
                vaccineType: existing.VaccineType,
                vaccineDate: existing.VaccineDate);

        private static PatientVaccineInfo _CreateInfo(int patientId)
        {
            var faker = new Faker();
            return new PatientVaccineInfo(
                patientId,
                faker.Random.Replace("##########"),
                firstName: faker.Name.FirstName(),
                lastName: faker.Name.LastName(),
                status: "Invited",
                vaccineType: faker.Random.Enum<VaccineType>(),
                vaccineDate: faker.Date.BetweenOffset(DateTimeOffset.Now.AddDays(-100), DateTimeOffset.Now));
        }
    }
}