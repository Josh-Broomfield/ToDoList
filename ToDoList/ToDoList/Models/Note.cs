//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToDoList.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Note
    {
        public Note()
        {
            Delete = false;
        }

        [Key]//Needed for ajax added notes, otherwise they element keys (not id) will be duplicated and the value of one will repeat
        public int Id { get; set; }
        public int Task_Id { get; set; }
        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public bool Delete { get; set; }//added manually

        public virtual Task Task { get; set; }
    }
}
