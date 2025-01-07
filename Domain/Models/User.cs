namespace Domain.Models;
public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public User(string firstName, string lastName, string email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Validate();
    }
    public void Update(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Validate();
    }
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentException("Last name cannot be empty");
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            throw new ArgumentException("Email is invalid");
    }
}