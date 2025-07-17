using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NodaTime;
using NodaTime.Calendars;

namespace GambitoServer.Pages.ControleProducao;

public class IndexModel : PageModel
{
  [FromQuery(Name = "data")]
  public string? DataQuery { get; set; } = null;

  public YearMonth? Mes { get; set; } = null;

  public List<LocalDate[]>? Semanas { get; set; } = null;

  public string? Error { get; set; } = null;

  private readonly ILogger<IndexModel> _logger;

  public IndexModel(ILogger<IndexModel> logger)
  {
    _logger = logger;
  }

  public readonly IWeekYearRule weekYearRule = WeekYearRules.ForMinDaysInFirstWeek(
    4,
    IsoDayOfWeek.Sunday
  );

  public IActionResult? OnGet()
  {
    if (DataQuery is not null)
    {
      var parse_result = NodaTime.Text.YearMonthPattern.Iso.Parse(DataQuery);
      if (!parse_result.Success)
      {
        Error = "Invalid Date";
        return Redirect(".");
      }
      else
      {
        Mes = parse_result.Value;
      }
    }
    Mes ??= new YearMonth(new DateOnly().Year, new DateOnly().Month);

    Semanas = [];

    var primeiro = true;

    foreach (var _date in Mes.Value.ToDateInterval())
    {
      var date = _date;
      if (primeiro)
      {
        primeiro = false;
        while (date.DayOfWeek != IsoDayOfWeek.Sunday)
        {
          date = date.PlusDays(-1);
        }
      }
      if (date.DayOfWeek == IsoDayOfWeek.Sunday)
      {
        Semanas.Add(
          [
            date,
            date.PlusDays(1),
            date.PlusDays(2),
            date.PlusDays(3),
            date.PlusDays(4),
            date.PlusDays(5),
            date.PlusDays(6),
          ]
        );
      }
    }
    return Page();
  }
}
