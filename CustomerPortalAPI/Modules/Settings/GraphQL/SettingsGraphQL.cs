using CustomerPortalAPI.Modules.Settings.Entities;
using CustomerPortalAPI.Modules.Settings.Repositories;
using HotChocolate;
using CustomerPortalAPI.Modules.Shared;

namespace CustomerPortalAPI.Modules.Settings.GraphQL
{
    public record TrainingOutput(int Id, string TrainingName, string? Description, string? TrainingType, bool IsActive, DateTime CreatedDate);
    public record CreateTrainingInput(string TrainingName, string TrainingCode, string? Description, string? TrainingType);
    public record UpdateTrainingInput(int Id, string? TrainingName, string? Description, bool? IsActive);
    public record CreateTrainingPayload(TrainingOutput? Training, string? Error);
    public record UpdateTrainingPayload(TrainingOutput? Training, string? Error);
    [ExtendObjectType("Query")]
    public class SettingsQueries
    {
        public async Task<IEnumerable<TrainingOutput>> GetTrainings([Service] ITrainingRepository repository)
        {
            var trainings = await repository.GetAllAsync();
            return trainings.Select(t => new TrainingOutput(t.Id, t.TrainingName, t.Description, t.TrainingType, t.IsActive, t.CreatedDate));
        }

        public async Task<TrainingOutput?> GetTrainingById(int id, [Service] ITrainingRepository repository)
        {
            var training = await repository.GetByIdAsync(id);
            return training == null ? null : new TrainingOutput(training.Id, training.TrainingName, training.Description, training.TrainingType, training.IsActive, training.CreatedDate);
        }
    }

    [ExtendObjectType("Mutation")]
    public class SettingsMutations
    {
        public async Task<CreateTrainingPayload> CreateTraining(CreateTrainingInput input, [Service] ITrainingRepository repository)
        {
            try
            {
                var training = new Training
                {
                    TrainingName = input.TrainingName,
                    TrainingCode = input.TrainingCode,
                    Description = input.Description,
                    TrainingType = input.TrainingType,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(training);
                return new CreateTrainingPayload(new TrainingOutput(created.Id, created.TrainingName, created.Description, created.TrainingType, created.IsActive, created.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new CreateTrainingPayload(null, ex.Message);
            }
        }

        public async Task<UpdateTrainingPayload> UpdateTraining(UpdateTrainingInput input, [Service] ITrainingRepository repository)
        {
            try
            {
                var training = await repository.GetByIdAsync(input.Id);
                if (training == null) return new UpdateTrainingPayload(null, "Training not found");

                if (input.TrainingName != null) training.TrainingName = input.TrainingName;
                if (input.Description != null) training.Description = input.Description;
                if (input.IsActive.HasValue) training.IsActive = input.IsActive.Value;
                training.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(training);
                return new UpdateTrainingPayload(new TrainingOutput(training.Id, training.TrainingName, training.Description, training.TrainingType, training.IsActive, training.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new UpdateTrainingPayload(null, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteTraining(int id, [Service] ITrainingRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }
    }
}

