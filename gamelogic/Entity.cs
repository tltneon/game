﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace gamelogic
{
    // Connecting to DB
    public class Entities : DbContext
    {
        public Entities() : base("DbConnection") { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Base> Bases { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Squad> Squads { get; set; }
    }
    // Tables
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public short Role { get; set; }
    }
    public class Player
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
    }
    public class Base
    {
        [Key]
        public int BaseID { get; set; }
        public string Basename { get; set; }
        public int OwnerID { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int Level { get; set; }
        public string CurrentTask { get; set; }
        public int FinishTime { get; set; }
        public bool IsActive { get; set; }
        public string Units { get; set; }
    }
    public class Building
    {
        [Key]
        public int BaseID { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public string CurrentTask { get; set; }
        public int FinishTime { get; set; }
    }
    public class Squad
    {
        [Key]
        public string Key { get; set; }
        public int OwnerID { get; set; }
        public int MoveFrom { get; set; }
        public int StartTime { get; set; }
        public int MoveTo { get; set; }
        public int FinishTime { get; set; }
        public string Units { get; set; }
    }
}
