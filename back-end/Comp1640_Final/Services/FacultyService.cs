using Comp1640_Final.Data;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{
    public interface IFacultyService
    {
        ICollection<Faculty> GetFaculties();
        ICollection<DashboardResponse> GetContributionsByYear();
        ICollection<DashboardResponse> GetContributorsByYear();
    }

    public class FacultyService : IFacultyService
    {
        public readonly ProjectDbContext _context;

        public FacultyService(ProjectDbContext context)
        {
            _context = context;
        }

        public ICollection<Faculty> GetFaculties()
        {
            return _context.Faculties.OrderBy(p => p.Id).ToList();
        }

        public ICollection<DashboardResponse> GetContributionsByYear()
        {
            var facultiesWithArticles = _context.Faculties
                .Include(f => f.Articles)
                .ToList();

            var contributionsByYear = new Dictionary<int, Dictionary<string, int>>();

            // Get distinct faculty names
            var facultyNames = facultiesWithArticles.Select(f => f.Name).Distinct().ToList();

            // Initialize contributionsByYear with all faculty names
            foreach (var year in facultiesWithArticles.SelectMany(f => f.Articles)
                                                      .Select(a => a.UploadDate.Year)
                                                      .Distinct())
            {
                contributionsByYear[year] = new Dictionary<string, int>();

                foreach (var facultyName in facultyNames)
                {
                    contributionsByYear[year][facultyName] = 0;
                }
            }

            // Populate contributionsByYear with actual contribution values
            foreach (var faculty in facultiesWithArticles)
            {
                var contributionsForFaculty = faculty.Articles
                    .GroupBy(a => a.UploadDate.Year)
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var year in contributionsByYear.Keys)
                {
                    if (contributionsForFaculty.ContainsKey(year))
                    {
                        contributionsByYear[year][faculty.Name] = contributionsForFaculty[year];
                    }
                }
            }

            // Convert contributionsByYear to DashboardResponse objects
            var dashboardResponses = contributionsByYear.Select(year => new DashboardResponse
            {
                Year = year.Key.ToString(),
                Values = year.Value.Select(kv => new Values { Faculty = kv.Key, value = kv.Value }).ToList()
            }).ToList();

            return dashboardResponses;
        }






        public ICollection<DashboardResponse> GetContributorsByYear()
        {
            var facultiesWithArticles = _context.Faculties
                .Include(f => f.Articles)
                .ToList();

            var contributorsByYear = new Dictionary<int, Dictionary<string, HashSet<string>>>();

            // Get distinct faculty names
            var facultyNames = facultiesWithArticles.Select(f => f.Name).Distinct().ToList();

            // Initialize contributorsByYear with all faculty names
            foreach (var year in facultiesWithArticles.SelectMany(f => f.Articles)
                                                      .Select(a => a.UploadDate.Year)
                                                      .Distinct())
            {
                contributorsByYear[year] = new Dictionary<string, HashSet<string>>();

                foreach (var facultyName in facultyNames)
                {
                    contributorsByYear[year][facultyName] = new HashSet<string>();
                }
            }

            // Populate contributorsByYear with actual contributor values
            foreach (var faculty in facultiesWithArticles)
            {
                var contributorsForFaculty = faculty.Articles
                    .GroupBy(a => a.UploadDate.Year)
                    .ToDictionary(g => g.Key, g => g.Select(a => a.StudentId).ToHashSet());

                foreach (var year in contributorsByYear.Keys)
                {
                    if (contributorsForFaculty.ContainsKey(year))
                    {
                        contributorsByYear[year][faculty.Name] = contributorsForFaculty[year];
                    }
                }
            }

            // Convert contributorsByYear to DashboardResponse objects
            var dashboardResponses = contributorsByYear.Select(year => new DashboardResponse
            {
                Year = year.Key.ToString(),
                Values = year.Value.Select(kv => new Values { Faculty = kv.Key, value = kv.Value.Count }).ToList()
            }).ToList();

            return dashboardResponses;
        }
    }

    }
