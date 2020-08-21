using Askmethat.Aspnet.JsonLocalizer.Localizer;
using Blogifier.Core.Data;
using Blogifier.Core.Helpers;
using Blogifier.Core.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogifier.Widgets
{
    public partial class ChartUnique
    {
        [Inject]
        protected IDataService DataService { get; set; }
        protected string[] Labels { get; set; }
        protected double[] Values { get; set; }
        [Inject]
        IJsonStringLocalizer<StatsUnique> Localizer { get; set; }

        protected List<ChartOptionUnique> ChartOptionUnique = new List<ChartOptionUnique>() {
            new ChartOptionUnique { Id = ChartSelectorUnique.Week, Label = "Last week" },
            new ChartOptionUnique { Id = ChartSelectorUnique.Month, Label = "Last month" },
            new ChartOptionUnique { Id = ChartSelectorUnique.All, Label = "All" }
        };

        private ChartSelectorUnique selectedChart;
        public ChartSelectorUnique SelectedChartOptionUnique
        {
            get { return selectedChart; }
            set
            {
                selectedChart = value;
                Load();
            }
        }

        protected override void OnInitialized()
        {
            SelectedChartOptionUnique = ChartSelectorUnique.Week;
        }

        void Load()
        {
            if (selectedChart == ChartSelectorUnique.Week)
            {
                LoadWeekly();
            }
            else if (selectedChart == ChartSelectorUnique.Month)
            {
                LoadMonthly();
            }
            else
            {
                LoadAll();
            }
        }

        void LoadWeekly()
        {
            var stats = GetStats(-7);
            var statsUnique = stats.GroupBy(s => s.DateCreated).Select(g => new { DateCreated = g.Key, Count = g.Select(l => l.Guid).Distinct().Count() });
            var chartItemsUnique = new List<ChartItemUnique>();

            foreach (var stat in statsUnique)
            {
                var date = stat.DateCreated.ToShortDateString();
                var item = chartItemsUnique.Where(i => i.Label == date).FirstOrDefault();

                if (item == null)
                {
                    chartItemsUnique.Add(new ChartItemUnique { Label = date, Value = stat.Count});
                }
                else
                {
                    item.Value += stat.Count;
                }
            }

            Labels = chartItemsUnique.Select(i => i.Label).ToArray();
            Values = chartItemsUnique.Select(i => i.Value).ToArray();
        }

        void LoadMonthly()
        {
            var stats = GetStats(-31);
            var statsUnique = stats.GroupBy(s => s.DateCreated).Select(g => new { DateCreated = g.Key, Count = g.Select(l => l.Guid).Distinct().Count() });
            var chartItemsUnique = new List<ChartItemUnique>();

            foreach (var stat in statsUnique)
            {
                var date = $"{stat.DateCreated.Month}/{stat.DateCreated.Day}";
                var item = chartItemsUnique.Where(i => i.Label == date).FirstOrDefault();

                if (item == null)
                {
                    chartItemsUnique.Add(new ChartItemUnique { Label = date, Value = stat.Count });
                }
                else
                {
                    item.Value += stat.Count;
                }
            }

            Labels = chartItemsUnique.Select(i => i.Label).ToArray();
            Values = chartItemsUnique.Select(i => i.Value).ToArray();
        }

        void LoadAll()
        {
            var stats = GetStats();
            var statsUnique = stats.GroupBy(s => s.DateCreated).Select(g => new { DateCreated = g.Key, Count = g.Select(l => l.Guid).Distinct().Count()});
            var chartItemsUnique = new List<ChartItemUnique>();

            foreach (var stat in statsUnique)
            {
                var date = $"{stat.DateCreated.Month}/{stat.DateCreated.Day}";
                var item = chartItemsUnique.Where(i => i.Label == date).FirstOrDefault();

                if (item == null)
                {
                    chartItemsUnique.Add(new ChartItemUnique { Label = date, Value = stat.Count });
                }
                else
                {
                    item.Value += stat.Count;
                }
            }

            Labels = chartItemsUnique.Select(i => i.Label).ToArray();
            Values = chartItemsUnique.Select(i => i.Value).ToArray();
        }

        IEnumerable<StatsUnique> GetStats(int days = 0)
        {
            IEnumerable<StatsUnique> stats = days == 0 ? DataService.StatsUniqueRepository.All() :
                DataService.StatsUniqueRepository.Find(p => p.DateCreated >= SystemClock.Now().Date.AddDays(days));
            return stats.OrderBy(s => s.DateCreated);
        }

        async Task LoadLatestPosts()
        {
            var posts = await Task.FromResult(DataService.BlogPosts
                .Find(p => p.Published > DateTime.MinValue)
                .OrderByDescending(p => p.Published));

            Labels = posts.Select(p => p.Title.Length > 4 ? p.Title.Substring(0, 5) + ".." : p.Title).ToArray();
            Values = posts.Select(p => (double)p.PostViews).ToArray();

            Array.Reverse(Labels);
            Array.Reverse(Values);

            if (Labels.Length > 10)
            {
                int skip = Labels.Length - 10;
                Values = Values.Skip(skip).ToArray();
                Labels = Labels.Skip(skip).ToArray();
            }
        }
    }
    public class ChartItemUnique
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }

    public class ChartOptionUnique
    {
        public ChartSelectorUnique Id { get; set; }
        public string Label { get; set; }
    }

    public enum ChartSelectorUnique
    {
        Week = 1, Month = 2, All = 3
    }
}
