using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Exceptions;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Application.Interfaces.Services;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class WeeklyStatusRichTextService : IWeeklyStatusRichTextService
    {
        private readonly IWeeklyStatusRichTextRepository _repository;

        public WeeklyStatusRichTextService(IWeeklyStatusRichTextRepository repository)
        {
            _repository = repository;
        }

        public async Task<WeeklyStatusRichTextDTO> GetWeeklyStatusByMemberByStartDateAsync(int memberId, int teamId, DateTime startDate)
        {
            if (memberId == 0 || teamId == 0)
                return null;
            else
            {
                var utcStartDate = startDate.ToUniversalTime();
                var weeklyStatus = await _repository.GetWeeklyStatusByMemberByStartDateAsync(memberId, teamId, utcStartDate);

                if (weeklyStatus == null)
                    return null;
                else
                {
                    var weeklyStatusDto = new WeeklyStatusRichTextDTO
                    {
                        Id = weeklyStatus.Id,
                        WeekStartDate = weeklyStatus.WeekStartDate,
                        Blockers = weeklyStatus.Blockers,
                        UpcomingPTO = weeklyStatus.UpcomingPTO,
                        MemberId = weeklyStatus.MemberId,
                    };

                    return weeklyStatusDto;
                }
            }
        }

        public async Task<IEnumerable<WeeklyStatusWithMemberNameDTO>> GetAllWeeklyStatusesByStartDateAsync(int teamId, DateTime weekStartDate)
        {
            var teamWeeklyStatuses = await _repository.GetAllWeeklyStatusesByDateAsync(teamId, weekStartDate);
            return teamWeeklyStatuses;
        }

        public async Task<WeeklyStatusRichTextDTO> AddWeeklyStatusAsync(WeeklyStatusRichTextDTO weeklyStatusDto)
        {
            var weeklyStatus = new WeeklyStatusRichText
            {
                WeekStartDate = weeklyStatusDto.WeekStartDate,
                Blockers = weeklyStatusDto.Blockers,
                UpcomingPTO = weeklyStatusDto.UpcomingPTO,
                MemberId = weeklyStatusDto.MemberId,
                TeamId = weeklyStatusDto.TeamId,
                CreatedDate = DateTime.UtcNow,
            };

            var addedStatus = await _repository.AddWeeklyStatusAsync(weeklyStatus);

            weeklyStatusDto.Id = addedStatus.Id;
            return weeklyStatusDto;
        }

        public async Task<WeeklyStatusRichTextDTO> UpdateWeeklyStatusAsync(WeeklyStatusRichTextDTO weeklyStatusDto)
        {
            var existingStatus = await _repository.GetWeeklyStatusByIdAsync((int)weeklyStatusDto.Id);

            if (existingStatus == null)
            {
                throw new WeeklyStatusNotFoundException(weeklyStatusDto.Id);
            }

            existingStatus.WeekStartDate = weeklyStatusDto.WeekStartDate;
            existingStatus.Blockers = weeklyStatusDto.Blockers;
            existingStatus.UpcomingPTO = weeklyStatusDto.UpcomingPTO;
            existingStatus.CreatedDate = DateTime.UtcNow;

            var updatedStatus = await _repository.UpdateWeeklyStatusAsync(existingStatus);

            return weeklyStatusDto;
        }
    }
}
