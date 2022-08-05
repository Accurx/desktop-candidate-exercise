using System;
using System.Collections.Immutable;

namespace DesktopApp
{
    public interface ISignalRService
    {
        IObservable<ImmutableArray<PatientVaccineInfo>> GetObservablePatientVaccineInfo();
    }
}