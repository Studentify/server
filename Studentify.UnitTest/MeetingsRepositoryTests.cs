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
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new MeetingsRepository(_context,
                                                 new SelectRepositoryBase<Meeting>(_context),
                                                 new InsertRepositoryBase<Meeting>(_context),
                                                 new UpdateRepositoryBase<Meeting>(_context));
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task TestInsertOneMeeting()
        {
            Meeting meeting = new Meeting()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe spotkanie insert",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                MaxNumberOfParticipants = 1
            };

            await _repository.Insert.One(meeting);
            var meetings = await _repository.Select.All();

            Assert.True(meetings.Contains(meeting));
        }

        [Test]
        public async Task TestGetOneMeeting()
        {
            Meeting meeting = new Meeting()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe spotkanie get one",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                MaxNumberOfParticipants = 1
            };

            await _repository.Insert.One(meeting);
            var selectedMeeting = await _repository.Select.ById(meeting.Id);

            Assert.AreEqual(meeting.Name, selectedMeeting.Name);
        }

        [Test]
        public async Task TestGetAllMeetings()
        {
            Meeting meeting = new Meeting()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe spotkanie get all",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                MaxNumberOfParticipants = 1
            };

            await _repository.Insert.One(meeting);
            var meetings = await _repository.Select.All();

            Assert.True(meetings.ToList().Count > 0);
        }

        [Test]
        public async Task TestUpdateOneMeeting()
        {
            const string newMeetingName = "Nowe testowe spotkanie";

            Meeting meeting = new Meeting()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe spotkanie insert",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                MaxNumberOfParticipants = 1
            };

            await _repository.Insert.One(meeting);

            meeting.Name = newMeetingName;
            await _repository.Update.One(meeting, meeting.Id);
            var updatedMeeting = await _repository.Select.ById(meeting.Id);

            Assert.AreEqual(newMeetingName, updatedMeeting.Name);
        }

        [Test]
        public async Task TestGetNoMeetings()
        {
            var meetings = await _repository.Select.All();

            Assert.False(meetings.Where(m => m.Name == "There is no meeting").Any());
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
