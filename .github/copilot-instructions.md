# copilot-instructions.md

Opinionated, concise guidance for generating .NET 9 code in this repo. Copilot, follow these conventions.

---

## TL;DR defaults

- Target .NET 9, C# 13. Use file-scoped namespaces, nullable enabled, async-first, DI everywhere, guard clauses.
- Use standard libraries and first-party Microsoft packages when possible (EF Core, Identity, MediatR, etc).
- Use minimal APIs or Razor Pages for web apps; ASP.NET Core Web API for APIs.
- Use EF Core with code-first migrations for data access; prefer async LINQ; no raw SQL.
- All services must implement interfaces; register with DI container as scoped by default.
- Logging is mandatory via Microsoft.Extensions.Logging with structured templates; never log secrets/PII.
- Validate inputs (DataAnnotations or FluentValidation). Return RFC 7807 ValidationProblem on bad input.
- Tests: unit tests with xUnit + FluentAssertions; integration tests with WebApplicationFactory; Testcontainers for infra.
- Comments: use XML documentation on all public classes and methods; at class level include Why (business requirement/intent).

---

## Project setup and Coding standards
- Use standard .NET project structure and conventions.
- Use file-scoped namespaces, top-level Program.cs, and primary constructors when they improve readability.
- Favor composition over inheritance; follow SOLID principles.
- Prefer `IReadOnlyList<T>`/`IEnumerable<T>` for outputs; `List<T>` only when mutating.
- Use `var` when the type is obvious from the right-hand side; otherwise, use explicit types.
- Use expression-bodied members for single-expression methods/properties.
- Use pattern matching and switch expressions for cleaner conditional logic.
- Use `async`/`await` for all I/O-bound operations; avoid blocking calls

---

## Security essentials

- Enforce HTTPS and HSTS (non-dev).
- Secrets: never commit. Use User Secrets (dev), env vars (containers), and managed secret stores.
- Don't trust user input; limit uploads and request sizes; validate content types; sanitize filenames.
- Persist DataProtection keys for multi-instance deployments.

---

## Logging guidelines

- Use structured logging: _logger.LogInformation("User {UserId} created employee {EmployeeId}", userId, id).
- Include correlation: Activity.Current?.Id ?? HttpContext.TraceIdentifier in errors.
- Don’t log PII/tokens/secrets. Use Warning for recoverable anomalies; Error for failures.
- Consider OpenTelemetry for traces/metrics/logs; instrument ASP.NET Core and HttpClient.

---

## Configuration

- Use AddOptions<T>().BindConfiguration(...).ValidateDataAnnotations().ValidateOnStart().
- Prefer environment-specific appsettings + user-secrets (dev) + env vars (deploy).
- Fail fast on invalid or missing configuration.

---

## Testing

- Unit: xUnit + FluentAssertions; mock with Moq.
- Integration: WebApplicationFactory<Program> + HttpClient.
- Infra: Testcontainers for SQLite/Postgres/Redis/etc.

---

## Commenting and documentation directives

- Enable XML documentation and add comments to all public classes and methods.
- Class-level comments should include a “Why” section summarizing business purpose/requirements.
- Methods should document parameters, return type, exceptions, and side effects.
- Keep comments updated when behavior or requirements change.

Example:

```csharp
/// <summary>
/// Retrieves a single employee by identifier if the caller is authorized.
/// </summary>
/// <remarks>
/// Why: FR-001: Only return the minimal data required by client apps to 
/// render employee lists and details without exposing sensitive HR fields.
/// </remarks>
/// <param name="id">Employee identifier.</param>
/// <returns>200 with the employee or 404 if not found or not owned by caller.</returns>
static async Task<Results<Ok<EmployeeDto>, NotFound>> GetEmployee(
    int id, IEmployeeRepository repo) { /* ... */ }
```
