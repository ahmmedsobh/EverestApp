using EverestApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface IMainService
    {
        Task<List<Article>> GetArticlesAysnc(bool forceRefresh = false);
    }
}
