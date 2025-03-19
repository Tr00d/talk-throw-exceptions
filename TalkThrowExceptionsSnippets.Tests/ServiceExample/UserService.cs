using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace TalkThrowExceptionsSnippets.Tests.ServiceExample;

public class UserService
{
    private readonly IEnumerable<User> dataStore = new List<User>();

    public User FindUser(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Cannot be null or empty", nameof(email));
        }

        return this.dataStore.FirstOrDefault(user => user.Email == email);
    }
}

public class UserServiceMonad
{
    private readonly IEnumerable<User> dataStore = new List<User>();

    public Either<Error, User> FindUser(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Either<Error, User>.Left(new ValidationFailed("Email cannot be null or empty"));
        }

        return this.dataStore.FirstOrDefault(user => user.Email == email)
               ?? Either<Error, User>.Left(new MissingUser($"User '{email}' is missing"));
    }
    
    public Either<Error, User> FindUserRefactored(string email) => ValidateEmail(email).Bind(this.LookupUser);

    private static Either<Error, string> ValidateEmail(string email) =>
        string.IsNullOrWhiteSpace(email)
            ? Either<Error, string>.Left(new ValidationFailed("Email cannot be null or empty"))
            : Either<Error, string>.Right(email);

    private Either<Error, User> LookupUser(string email) =>
        this.dataStore.FirstOrDefault(user => user.Email == email)
        ?? Either<Error, User>.Left(new MissingUser($"User '{email}' is missing"));
}

public abstract record Error(string Reason);
public record ValidationFailed(string Reason) : Error(Reason);
public record MissingUser(string Reason) : Error(Reason);


public record User(string Email);