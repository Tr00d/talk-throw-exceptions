# Employee Onboarding (C#)

## Setup

Requires the .NET SDK 8.0.

```bash
cd Workshop/csharp
dotnet restore
```

## Running the tests

```bash
dotnet test   # build and run all tests
dotnet build  # build only
```

## The system

`Onboarding.ProcessNewHires(params Department[] departments)` goes through a
list of departments and onboards the approved candidate of each one. For a
single department there are three steps, each depending on the previous one:

1. Find the approved candidate for the department (`ICandidateRepository`).
2. Generate their contract (`IHrSystem`).
3. Provision their IT account (`IItProvisioning`).

If all three succeed, a confirmation string is added to the result. The method
returns the confirmations for every department that completed all three steps.

## The problem

The code in `EmployeeOnboarding/Onboarding.cs` works on the happy path but has
several issues:

- Every step returns a nullable (`Candidate?`, `Contract?`, `Account?`). A
  missing candidate, a missing contract, and a missing account are three
  different failures, but the caller can't tell them apart.
- When a step returns null, the department is skipped with no record and no
  report. The result list is just shorter and there's no way to know why.
- Error handling is inconsistent. The "no departments" case throws an
  `ArgumentException`, the other three fail silently.
- The three steps are written as nested `if` blocks.
- The return type is `List<string>`, so it can only describe what succeeded, not
  what failed.

The test suite in `EmployeeOnboarding.Tests/OnboardingTests.cs` shows the
consequence: three different failures all assert the same empty result. The
tests can't tell the failures apart because the code can't either.

## The exercise

Make each outcome (success and each failure) visible in the type system, so the
result is predictable and every case is handled. Refactor the process using
monads from an existing library:

- [language-ext](https://github.com/louthy/language-ext)
- [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions)
- [Vonage](https://github.com/Vonage/vonage-dotnet-sdk), if you want to
  reuse what you saw in the talk. It's technically not a functional library, but it contains a full set you can reuse.

Use whichever you prefer.

## Hints

- A lookup that may return nothing maps to `Option` / `Maybe` (`Some` / `None`).
- A step that succeeds or fails with a reason maps to `Either` / `Result`
  (success vs. a typed failure). Model the failure reasons explicitly instead of
  using null.
- Chaining dependent steps that can stop early is `Bind` / `Map`, which replaces
  the nested `if` blocks.
- Processing the list of departments where each returns an outcome is
  `Traverse` / `Sequence`. Decide whether one failing department aborts
  everything or whether you collect every outcome.
- Once failures are typed, update the tests to assert which failure occurred.
