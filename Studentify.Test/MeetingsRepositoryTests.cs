using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models;
using Studentify.Models.Authentication;
using Studentify.Models.StudentifyEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studentify.Test
{
    class MeetingsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private MeetingsRepository _repository;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            StudentifyDbContext context = new StudentifyDbContext(_dbContextOptions);
            _repository = new MeetingsRepository(context,
                                                 new SelectRepositoryBase<Meeting>(context),
                                                 new InsertRepositoryBase<Meeting>(context),
                                                 new UpdateRepositoryBase<Meeting>(context));
        }

        [Test]
        public async Task TestInsertOneMeeting()
        {
            Meeting meeting = new Meeting()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe spotkanie",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                MaxNumberOfParticipants = 1
            };

            await _repository.Insert.One(meeting);
            var meetings = await _repository.Select.All();

            Assert.True(meetings.Contains(meeting));
        }

        [Test]
        public async Task TestRegisterAttendance()
        {
            const int expectedNumberOfParticipants = 1;
            Meeting meeting = new Meeting()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe spotkanie",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                MaxNumberOfParticipants = 2,
                Participants = new List<StudentifyAccount>()
            };
            StudentifyAccount account = new StudentifyAccount();

            await _repository.Insert.One(meeting);
            await _repository.RegisterAttendance(meeting, account);

            var selectedMeeting = await _repository.Select.ById(meeting.Id);
            Assert.AreEqual(expectedNumberOfParticipants, selectedMeeting.Participants.Count);
        }
    }
}
