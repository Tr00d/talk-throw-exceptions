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

`Onboarding.OnboardNewHire(AcceptedOffer offer)` onboards a single new hire
through four sequential steps, each depending on the previous one:

1. Register the employee from the accepted offer (`IEmployeeRepository`).
2. Generate their contract (`IHrSystem`).
3. Provision their IT account (`IItProvisioning`).
4. Enroll them in payroll (`IPayroll`).

If all four succeed, the method returns an `OnboardingResult`.

## The problem

The code in `EmployeeOnboarding/Onboarding.cs` works on the happy path but has
several issues:

- Each step signals failure by throwing a typed exception
  (`EmployeeRegistrationException`, `ContractGenerationException`,
  `AccountProvisioningException`, `PayrollEnrollmentException`). Failure travels
  out of band instead of being part of the result.
- The signature (`OnboardingResult OnboardNewHire(AcceptedOffer)`) only
  describes success. Nothing in the type tells the caller the call can fail.
- Control flow runs through `try` / `catch`. The four steps live in one `try`
  block followed by four near-identical `catch` clauses.
- Every `catch` does the same thing: rethrow as a generic
  `BusinessException(e.Message)`. The four distinct failures collapse into one
  type, and the caller can only tell them apart by parsing a message string.

The test suite in `EmployeeOnboarding.Tests/OnboardingTests.cs` shows the
consequence: four different failures all assert the same `BusinessException`.
The tests can only tell the failures apart by their message.

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

- A step that succeeds or fails with a reason maps to `Either` / `Result`
  (success vs. a typed failure). Return the failure as a value instead of
  throwing an exception.
- Model the four failure reasons explicitly so the caller can tell them apart,
  instead of collapsing them into one `BusinessException`.
- Chaining dependent steps that stop on the first failure is `Bind` / `Map`,
  which replaces the `try` / `catch`.
- Make the signature honest: the return type should express "an
  `OnboardingResult` or a typed failure," not just the success shape.
- Once failures are typed, update the tests to assert which failure occurred.
