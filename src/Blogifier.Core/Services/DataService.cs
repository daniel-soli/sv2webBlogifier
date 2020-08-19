using Blogifier.Core.Data;
using Blogifier.Core.Data.Repositories;
using System;

namespace Blogifier.Core.Services
{
    public interface IDataService : IDisposable
    {
        IPostRepository BlogPosts { get; }
        IAuthorRepository Authors { get; }
        INotificationRepository Notifications { get; }
        IHtmlWidgetRepository HtmlWidgets { get; }
        ICustomFieldRepository CustomFields { get; }
        INewsletterRepository Newsletters { get; }
        IStatsRepository StatsRepository { get; }
        IStatsUniqueRepository StatsUniqueRepository { get; }

        int Complete();
    }

    public class DataService : IDataService
    {
        private readonly AppDbContext _db;

        public DataService(
            AppDbContext db,
            IPostRepository postRepository,
            IAuthorRepository authorRepository,
            INotificationRepository notificationRepository,
            IHtmlWidgetRepository htmlWidgetRepository,
            ICustomFieldRepository customFieldRepository,
            INewsletterRepository newsletterRepository,
            IStatsRepository statsRepository,
            IStatsUniqueRepository statsUniqueRepository)
        {
            _db = db;

            BlogPosts = postRepository;
            Authors = authorRepository;
            Notifications = notificationRepository;
            HtmlWidgets = htmlWidgetRepository;
            CustomFields = customFieldRepository;
            Newsletters = newsletterRepository;
            StatsRepository = statsRepository;
            StatsUniqueRepository = statsUniqueRepository;
        }

        public IPostRepository BlogPosts { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IHtmlWidgetRepository HtmlWidgets { get; private set; }
        public ICustomFieldRepository CustomFields { get; private set; }
        public INewsletterRepository Newsletters { get; private set; }
        public IStatsRepository StatsRepository { get; private set; }
        public IStatsUniqueRepository StatsUniqueRepository { get; private set; }

        public int Complete()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}