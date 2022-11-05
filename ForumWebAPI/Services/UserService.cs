using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace ForumWebAPI;

public class UserService
{   
    private readonly DataContext dataContext;
    private UserRepo ur;

    public UserService(DataContext dataContext)
    {
        this.dataContext = dataContext;
        ur = new UserRepo(dataContext);
    }

    #region CRUD
    public async Task<List<User>> AddUser(User u){
        if(!CheckUser(u)){
            throw new ArgumentException();
        }
        return await ur.AddUser(u);
    }

    public async Task<List<User>> UpdateUser(User u){
        if(!CheckUser(u)){
            throw new ArgumentException();
        }
        var dbUser = await ur.GetUser(u.Id);
        if(dbUser == null){
            throw new ArgumentException();
        }
        dbUser.Name = u.Name;
        dbUser.Surname = u.Surname;
        dbUser.Country = u.Country;
        dbUser.BirthDate = u.BirthDate;
        dbUser.email = u.email;
        //dodać jeśli nie chce updatować jednego lub wielu z pól żeby przypadkiem nulla nie przypisać
        return await ur.UpdateUser();
    }

    public async Task<User> GetUser(int id){
        var User = await ur.GetUser(id);
        if(User == null){
            throw new ArgumentException();
        }
        return User;
    }

    public async Task<List<User>> DeleteUser(int id){
        var dbUser = await ur.GetUser(id);
        if(dbUser == null){
            throw new ArgumentException();
        }
        return await ur.DeleteUser(dbUser);
    }

    public async Task<List<User>> GetUsers(){
        var UserList = await ur.GetUsers();
        if(UserList == null || UserList.Count <= 0){
            throw new ArgumentException();
        }
        return UserList;
    }

    #endregion

    #region Checkers
    private bool CheckUser(User p){
        bool IsValid = true;
        if(p == null){
            IsValid = false;
            return IsValid;
        } else if(!p.email.Contains("@") || !p.email.Contains(".")){
            IsValid = false;
            return IsValid;
        } else if (CheckRegex(p.Name) || CheckRegex(p.Surname) || CheckRegex(p.Country)){
            IsValid = false;
            return IsValid;
        }
        return IsValid;
    }

    private bool CheckRegex(string s){
        bool IsBadString = true;
        if(string.IsNullOrEmpty(s)){
            return IsBadString;
        } else if (!Regex.IsMatch(s, @"(^[a-zA-Z]+$)|(^[a-zA-Z]-[a-zA-Z]$)")){
            return IsBadString;
        }
        foreach (char c in s){
            if(!Char.IsLetter(c)){
                if(c == '-'){
                    continue;
                }
                return IsBadString;
            }
        }
        IsBadString = false;
        return IsBadString;
    }
    #endregion
}