# Quiz Maker I — C# Console Exercise

**Status: historical programming practice**, not a production educational service or verified portfolio application.

The project explores separating a quiz application's responsibilities:
- `QUIZ Maker I/Program.cs` — console entry point.
- `QUIZ Maker I/QuizManager.cs` — quiz control flow.
- `QUIZ Maker I/Question.cs` — question data structure.
- `QUIZ Maker I/UI.cs`, `Messages.cs`, `Constants.cs` — user interaction and strings.

The solution targets **.NET 7** (now out of support). Build and test execution have **not** been independently verified during this audit.

Explore using a compatible SDK:
```bash
dotnet restore "QUIZ Maker I.sln"
dotnet run --project "QUIZ Maker I/QUIZ Maker I.csproj"
```

Ideas for future tests: scoring boundaries, invalid answer inputs, question ordering and missing-question handling. These are proposals only.

For current QA automation work with verified CI, see [CommerceQA / FarmFresh](https://github.com/Esoham/QA-Automation-Project).
