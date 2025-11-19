using System;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IEmployeeAccessWorkshopJobType
    {
        String JobTypeId { get; }

        Boolean CanAccessJobType();

        Boolean CanPerformJobTransition(JobStatus jobSourceStatus, JobStatus jobDestinationStatus);

        Boolean CanPerformHandpieceTransition(JobStatus jobStatus, HandpieceStatus handpieceSourceStatus, HandpieceStatus handpieceDestinationStatus);

        Boolean CanReadJobComponent(JobEntityComponent jobComponent);

        Boolean CanWriteJobComponent(JobEntityComponent jobComponent);

        Boolean CanReadJobField(JobStatus jobStatus, JobEntityField jobField);

        Boolean CanWriteJobField(JobStatus jobStatus, JobEntityField jobField);

        Boolean CanInitJobField(JobEntityField jobField);

        Boolean CanReadHandpieceComponent(HandpieceEntityComponent handpieceComponent);

        Boolean CanWriteHandpieceComponent(HandpieceEntityComponent handpieceComponent);

        Boolean CanReadHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField);

        Boolean CanWriteHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField);

        Boolean CanInitHandpieceField(HandpieceEntityField handpieceField);
    }
}
