using System.Collections.Generic;
using System.Linq;
using Fanex.Tarus.Entities.Report;

namespace Fanex.Tarus.Web.ViewModel.Report.Models
{
    public class LiveCasinoWinLossViewModel
    {
        public LiveCasinoWinLoss LiveCasinoWinLoss;
        public GameSearch GameSearchViewModel;

    }

    public class TimePeriod
    {
        public int ID { get; set; }
        public string Value { get; set; }

        public static IQueryable<TimePeriod> GetTimePeriod()
        {
            return new List<TimePeriod>
            {
                new TimePeriod {
                    ID = 1,
                    Value = "Today"
                },
                new TimePeriod {
                    ID = 2,
                    Value = "Yesterday"
                },
               new TimePeriod {
                    ID = 3,
                    Value = "Current week"
                },
                new TimePeriod {
                    ID = 4,
                    Value = "Last week"
                },
                new TimePeriod {
                    ID = 5,
                    Value = "Current month"
                },
                new TimePeriod {
                    ID = 6,
                    Value = "Last month"
                },
                new TimePeriod {
                    ID = 7,
                    Value = "Since last month"
                },
                new TimePeriod {
                    ID = 8,
                    Value = "Last 3 months"
                },
                new TimePeriod {
                    ID = 9,
                    Value = "Last 6 month"
                }, 
                new TimePeriod {
                    ID = 0,
                    Value = "Specific period"
                }
               
            }.AsQueryable();
        }
    }


}