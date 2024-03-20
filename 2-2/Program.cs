using Microsoft.EntityFrameworkCore;


void task1() {
    decimal minCost = 25000000;
    decimal maxCost = 35000000;
    string regionName = "Волжский";
    Console.WriteLine("Задание 1:");

    using (var context = new AppDbContext())
    {
        var estatesInRegion = context.Estates
                .Where(c => c.Cost > minCost && c.Cost < maxCost && c.Region.Name == regionName)
                .Select(ed => new { ID = ed.ID, Address = ed.Address, Cost = ed.Cost }).ToList();

        foreach (var estate in estatesInRegion)
        {
            Console.WriteLine($"Код: {estate.ID}, адрес: {estate.Address}, стоимость: {estate.Cost}");
        }
    }
}

void task2(){
    int num = 2;
    Console.WriteLine("Задание 2:");
    using (var context = new AppDbContext()) 
    {
        var relAgents = context.Deals
            .Where(e => e.Estate.RoomNum == num)
            .Select(s => new {Surname = s.Agent.Surname, Name = s.Agent.Name, Patronymic = s.Agent.Patronymic});
        foreach (var ag in relAgents)
            {
                Console.WriteLine($"Фамилия: {ag.Surname}, имя: {ag.Name}, отчество: {ag.Patronymic}");
            }
    }

}

void task3() 
{
    Console.WriteLine("Задание 3:");
    int roomNumber = 2;
        string regionName = "Октябрьский";

        using (var context = new AppDbContext())
        {
            var totalCost = context.Estates
                .Where(estate => estate.RoomNum == roomNumber && estate.Region.Name == regionName)
                .Join(context.Deals,
                      estate => estate.ID,
                      deal => deal.Estate.ID,
                      (estate, deal) => deal.Cost)
                .Sum();

            Console.WriteLine($"Общая стоимость объектов недвижимости в районе \"{regionName}\", имеющих {roomNumber} комнаты, составляет {totalCost}");
        }
}

void task4()
{
    Console.WriteLine("Задание 4:");
    string realtorID = "Иванов";

        using (var context = new AppDbContext())
        {
            var realtorDeals = context.Deals
                .Where(deal => deal.Agent.Surname.Trim() == realtorID)
                .Select(c => c.Estate.Cost);

            decimal maxCost = realtorDeals.Max();
            decimal minCost = realtorDeals.Min();

            Console.WriteLine($"Максимальная стоимость объекта недвижимости, проданного риэлтором {realtorID}, составляет {maxCost}");
            Console.WriteLine($"Минимальная стоимость объекта недвижимости, проданного риэлтором {realtorID}, составляет {minCost}");
        }
}

void task5()
{
    Console.WriteLine("Задание 5:");
    string realtorName = "Иванов";
    string propertyType = "апартаменты";
    string criterionName = "безопасность";

    using (var context = new AppDbContext())
    {
        var averageRating = context.Ratings
            .Join(context.Deals,
                    rating => rating.Estate.ID,
                    deal => deal.Estate.ID,
                    (rating, deal) => new { Rating = rating, Deal = deal })
            .Where(rd => rd.Deal.Agent.Surname == realtorName &&
                            rd.Rating.Estate.Tip.Name == propertyType &&
                            rd.Rating.Criterion.Name == criterionName)
            .Select(rd => rd.Rating.Rate)
            .Average();

        Console.WriteLine($"Средняя оценка апартаментов по критерию \"{criterionName}\", проданных риэлтором {realtorName}, составляет {averageRating}");
    }
}

void task6()
{
    Console.WriteLine("Задание 6:");
    int lvl = 2;
    string estTip = "квартира";
    using (var context = new AppDbContext()) {
        var estCnt = context.Estates
            .Where(c => c.Level == lvl && c.Tip.Name == estTip)
            .Select(r => new {Region = r.Region})
            .GroupBy(info => info.Region)
            .Select(s => new {Region = s.Key, cnt = s.Count()});
        foreach (var d in estCnt){
            Console.WriteLine($"Район {d.Region.Name} имеет {d.cnt} квартир на втором этаже");
        }
    }
}

void task7()
{
    Console.WriteLine("Задание 7:");
    string buildType = "квартира";
    using (var context = new AppDbContext()) {
        var kvCnt = context.Deals
            .Where(et => et.Estate.Tip.Name == buildType)
            .GroupBy(p => p.Agent.Surname)
            .Select(c => new {Agent = c.Key, cnt = c.Count()});
        foreach (var d in kvCnt){
            Console.WriteLine($"Агент {d.Agent} продал {d.cnt} квартир."); 
        }
    }
}

void task8()
{
    Console.WriteLine("Задание 8:");
    using (var context = new AppDbContext())
    {
        var expObj = context.Estates
            .GroupBy(r => r.Region.Name)
            .Select(g => new {Region = g.Key, res = g.OrderByDescending(c => c.Cost).Take(3)});
        foreach (var r in expObj) {
            Console.WriteLine($"Самые дорогие объекты района {r.Region}: ");
            foreach (var ans in r.res) {
                Console.WriteLine($"Адрес: {ans.Address}, стоимость: {ans.Cost}, этаж {ans.Level}");
            }
        }
    } 
}

void task9()
{
    Console.WriteLine("Задание 9:");
    using (var context = new AppDbContext())
    {
        var ans = context.Deals
            .GroupBy(a => new {Agent = a.Agent.Surname, Year = a.Data.Year})
            .Where(c => c.Count() > 1)
            .Select(g => new {group = g.Key, cnt = g.Count()});
        foreach (var a in ans) {
            Console.WriteLine($"Риэлтор {a.group.Agent} продал {a.cnt} объектов в {a.group.Year} году");
        }   
    }
}

void task10()
{
    Console.WriteLine("Задание 10:");
    using (var context = new AppDbContext())
    {
        var ch = context.Estates
            .GroupBy(d => new {d.Data.Year})
            .Where(c => c.Count() >= 2 && (c.Count() <= 3))
            .Select(g => new {group = g.Key, cnt = g.Count()});
        foreach (var a in ch){
            Console.WriteLine($"В {a.group.Year} году было размещено {a.cnt} объявления.");
        }
    }
}

void task11()
{
    Console.WriteLine("Задание 11:");
    using (var context = new AppDbContext()) {
        var rel = context.Deals
            .Join(context.Estates,
            deal => deal.Estate.ID,
            estate => estate.ID,
            (deal, estate) => new { Deal = deal, Estate = estate})
            .Where(cr => Math.Max(cr.Deal.Cost, cr.Estate.Cost) / Math.Min(cr.Deal.Cost, cr.Estate.Cost) <= 1.2)
            .Select(d => new {Deal = d.Deal, Estate = d.Estate, Region = d.Estate.Region.Name});
        foreach (var ans in rel) {
            Console.WriteLine($"Адрес: {ans.Estate.Address}, район: {ans.Region}");
        }
    }
}

void task12()
{
    Console.WriteLine("Задание 12:");
    using (var context = new AppDbContext())
        {
            var averageCostPerSquareMeterByRegion = context.Estates
                .GroupBy(r => r.Region.ID)
                .Select(x => x
                    .Where(e => ((double)e.Cost / e.Area) < ((double)x.Select(c => c.Cost / c.Area).Average())))
                .ToList()
                .Aggregate((prev, next) => prev.UnionBy(next, x => x.ID))
                .Select(estate => new
                {
                    Address = estate.Address,
                    CostPerSquareMeter = estate.Cost / estate.Area
                })
                ;
            foreach (var r in averageCostPerSquareMeterByRegion)
                {                
                    Console.WriteLine(r.Address);
                }
        }
}

void task13()
{
    Console.WriteLine("Задание 13:");
    int curYear = 2024;
    using (var context = new AppDbContext())
        {
            var rel = context.Agents
                .Where(agent => !context.Deals.Any(deal => deal.Agent.ID == agent.ID && deal.Data.Year == curYear))
                .Select(agent => new
                {
                    FullName = $"{agent.Surname} {agent.Name} {agent.Patronymic}"
                })
                .ToList();

            foreach (var realtor in rel)
            {
                Console.WriteLine($"Риэлтор: {realtor.FullName}");
            }
        }
}

void task14()
{
    Console.WriteLine("Задание 14:");
    int currentYear = DateTime.Now.Year;
        int previousYear = currentYear - 1;

        using (var context = new AppDbContext())
        {
            var salesByRegionCurrentYear = context.Deals
                .Where(deal => deal.Data.Year == currentYear)
                .GroupBy(deal => deal.Estate.Region.Name)
                .Select(group => new
                {
                    Region = group.Key,
                    SalesCountCurrentYear = group.Count()
                })
                .ToDictionary(region => region.Region, region => region.SalesCountCurrentYear);

            var salesByRegionPreviousYear = context.Deals
                .Where(deal => deal.Data.Year == previousYear)
                .GroupBy(deal => deal.Estate.Region.Name)
                .Select(group => new
                {
                    Region = group.Key,
                    SalesCountPreviousYear = group.Count()
                })
                .ToDictionary(region => region.Region, region => region.SalesCountPreviousYear);

            foreach (var region in salesByRegionCurrentYear.Keys)
            {
                int currentYearSales = salesByRegionCurrentYear.ContainsKey(region) ? salesByRegionCurrentYear[region] : 0;
                int previousYearSales = salesByRegionPreviousYear.ContainsKey(region) ? salesByRegionPreviousYear[region] : 0;

                double percentChange = (currentYearSales - previousYearSales) / (double)previousYearSales * 100;

                Console.WriteLine($"Район: {region}");
                Console.WriteLine($"Продаж в текущем году: {currentYearSales}");
                Console.WriteLine($"Продаж в предыдущем году: {previousYearSales}");
                Console.WriteLine($"Процент изменения: {percentChange}%");
                Console.WriteLine();
            }
        }
}

void task15() 
{
     Console.WriteLine("Задание 15:");
     int estateId = 1000001;

        using (var context = new AppDbContext())
        {
            var averageRatingByCriterion = context.Ratings
                .Where(rating => rating.Estate.ID == estateId)
                .GroupBy(rating => rating.Criterion.Name)
                .Select(group => new
                {
                    Criterion = group.Key,
                    AverageRating = group.Average(rating => rating.Rate)
                });

            Console.WriteLine("Критерий\tСредняя оценка\tТекст");

            foreach (var rating in averageRatingByCriterion)
            {
                string equivalentText = GetEquivalentText(rating.AverageRating);
                Console.WriteLine($"{rating.Criterion}\t{rating.AverageRating:F1} из 10\t{equivalentText}");
            }
        }
    }

    static string GetEquivalentText(double averageRating)
    {
        if (averageRating >= 9)
        {
            return "превосходно";
        }
        else if (averageRating >= 8)
        {
            return "очень хорошо";
        }
        else if (averageRating >= 7)
        {
            return "хорошо";
        }
        else if (averageRating >= 6)
        {
            return "удовлетворительно";
        }
        else
        {
            return "неудовлетворительно";
        }
}

void newtask()
{
    using (var context = new AppDbContext)
    {
        var ans = context.
        ;
    }
}

task1();
task2();
task3();
task4();
task5();
task6();
task7();
task8();
task9();
task10();
task11();
task12();
task13();
task14();
task15();
newtask();