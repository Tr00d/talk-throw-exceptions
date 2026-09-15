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

`Onboarding.processNewHires(...departments)` goes through a list of departments
and onboards the approved candidate of each one. For a single department there
are three steps, each depending on the previous one:

1. Find the approved candidate for the department (`CandidateRepository`).
2. Generate their contract (`HrSystem`).
3. Provision their IT account (`ItProvisioning`).

If all three succeed, a confirmation string is added to the result. The method
returns the confirmations for every department that completed all three steps.

## The problem

The code in `src/onboarding.ts` works on the happy path but has several issues:

- Every step returns `T | null`. A null candidate, a null contract, and a null
  account are three different failures, but the caller can't tell them apart.
- When a step returns null, the department is skipped with no record and no
  report. The result list is just shorter and there's no way to know why.
- Error handling is inconsistent. The "no departments" case throws, the other
  three fail silently.
- The three steps are written as nested `if` blocks.
- The return type is `string[]`, so it can only describe what succeeded, not
  what failed.

The test suite in `tests/onboarding.test.ts` shows the consequence: three
different failures all assert the same empty result. The tests can't tell the
failures apart because the code can't either.

## The exercise

Make each outcome (success and each failure) visible in the type system, so the
result is predictable and every case is handled. Refactor the process using
monads from an existing library:

- [fp-ts](https://gcanti.github.io/fp-ts/)
- [Effect](https://effect.website/)

Use whichever you prefer.

Some directions:

- A lookup that may return nothing maps to `Option` (`Some` / `None`).
- A step that succeeds or fails with a reason maps to `Either` (`Right` for
  success, `Left` for a typed failure). Model the failure reasons explicitly
  instead of using null.
- Chaining dependent steps that can stop early is `chain` / `flatMap`, which
  replaces the nested `if` blocks.
- Processing the list of departments where each returns an outcome is
  `traverse` / `sequence`. Decide whether one failing department aborts
  everything or whether you collect every outcome.
- Once failures are typed, update the tests to assert which failure occurred.
