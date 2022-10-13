namespace VerselineSelector.Domain.ChapterIV;

public enum ParagaphProcessType
{
    NotApplicable = 0,
    Synchronous = 1,
    Asynchronous = 2,
    NewRequestAsynchronousExtensionSynchronous = 3,
    NewRequestSynchronousExtensionAsynchronous = 4,
    NotificationProcedureOutPatient = 5,
    NotificationProcedureInPatient = 6,
    Paper = 7,
    DigitalRequestAndTardisMandatory = 8,
    DigitalRequestMandatory = 9,
}