using System;
using Xunit;
using Domain.Models;
using Domain.Enums;
namespace Tests.Models;
public class UserModelTests
{
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenFirstNameIsEmpty()
    {
        // Arrange
        string firstName = "";
        string lastName = "Smith";
        string email = "test@example.com";
        string phoneNumber = "123456789";
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = Role.Patient;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("First name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenLastNameIsEmpty()
    {
        // Arrange
        string firstName = "John";
        string lastName = "";
        string email = "test@example.com";
        string phoneNumber = "123456789";
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = Role.Patient;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("Last name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenEmailIsInvalid()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string email = "invalid-email"; // Invalid email
        string phoneNumber = "123456789";
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = Role.Patient;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("Email is invalid.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDateOfBirthIsInTheFuture()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string email = "test@example.com";
        string phoneNumber = "123456789";
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(1); // Future date of birth
        Role role = Role.Patient;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("Date of birth must be in the past.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenAddressIsEmpty()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string email = "test@example.com";
        string phoneNumber = "123456789";
        string address = ""; // Empty address
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = Role.Patient;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("Address cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenPhoneNumberIsEmpty()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string email = "test@example.com";
        string phoneNumber = ""; // Empty phone number
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = Role.Patient;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("Phone number cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenRoleIsInvalid()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string email = "test@example.com";
        string phoneNumber = "123456789";
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = (Role)999; // Invalid role

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role)
        );
        Assert.Equal("Invalid role assigned.", exception.Message);
    }

    [Fact]
    public void Constructor_CreatesUserSuccessfully_WhenAllFieldsAreValid()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string email = "test@example.com";
        string phoneNumber = "123456789";
        string address = "123 Street";
        string? postcode = "12345";
        DateTime dateOfBirth = DateTime.Now.AddYears(-20); // Valid age
        Role role = Role.Patient;

        // Act
        var user = new User(firstName, lastName, email, phoneNumber, address, postcode, dateOfBirth, role);

        // Assert
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(email, user.Email);
        Assert.Equal(phoneNumber, user.PhoneNumber);
        Assert.Equal(address, user.Address);
        Assert.Equal(postcode, user.PostCode);
        Assert.Equal(dateOfBirth, user.DateOfBirth);
        Assert.Equal(role, user.Role);
    }
}
