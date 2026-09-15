# Employee Onboarding (TypeScript)

## Setup

Requires Node.js 18+.

```bash
cd Workshop/ts
npm install
```

## Running the tests

```bash
npm test           # run once
npm run test:watch # re-run on change
npm run typecheck  # type-check only
```

## The system

`Onboarding.onboardNewHire(offer)` onboards a single new hire through four
sequential steps, each depending on the previous one:

1. Register the employee from the accepted offer (`EmployeeRepository`).
2. Generate their contract (`HrSystem`).
3. Provision their IT account (`ItProvisioning`).
4. Enroll them in payroll (`Payroll`).

If all four succeed, the method returns an `OnboardingResult`.

## The problem

The code in `src/onboarding.ts` works on the happy path but has several issues:

- Each step signals failure by throwing a typed error
  (`EmployeeRegistrationException`, `ContractGenerationException`,
  `AccountProvisioningException`, `PayrollEnrollmentException`). Failure travels
  out of band instead of being part of the result.
- The signature (`onboardNewHire(offer): OnboardingResult`) only describes
  success. Nothing in the type tells the caller the call can fail.
- Control flow runs through `try` / `catch`. The four steps live in one `try`
  block followed by a chain of near-identical `instanceof` checks.
- Every branch does the same thing: rethrow as a generic
  `BusinessException(message)`. The four distinct failures collapse into one
  type, and the caller can only tell them apart by parsing a message string.

The test suite in `tests/onboarding.test.ts` shows the consequence: four
different failures all assert the same `BusinessException`. The tests can only
tell the failures apart by their message.

## The exercise

Make each outcome (success and each failure) visible in the type system, so the
result is predictable and every case is handled. Refactor the process using
monads from an existing library:

- [fp-ts](https://gcanti.github.io/fp-ts/)
- [Effect](https://effect.website/)

Use whichever you prefer.

## Hints:

- A step that succeeds or fails with a reason maps to `Either` (`Right` for
  success, `Left` for a typed failure). Return the failure as a value instead of
  throwing an error.
- Model the four failure reasons explicitly so the caller can tell them apart,
  instead of collapsing them into one `BusinessException`.
- Chaining dependent steps that stop on the first failure is `chain` /
  `flatMap`, which replaces the `try` / `catch`.
- Make the signature honest: the return type should express "an
  `OnboardingResult` or a typed failure," not just the success shape.
- Once failures are typed, update the tests to assert which failure occurred.
