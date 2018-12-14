using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class NonPlayableCharacter : Character
    {
        public int gameid { get; set; }
    }
}
