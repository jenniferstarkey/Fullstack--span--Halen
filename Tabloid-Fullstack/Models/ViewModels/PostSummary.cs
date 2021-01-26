﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tabloid_Fullstack.Models.ViewModels
{
    public class PostSummary
    {
        public int Id { get; set; }
        public string ImageLocation { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public string AbbreviatedText { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public DateTime? PublishDateTime { get; set; }
        public string PreviewText => AbbreviatedText + "...";
        public Category Category { get; set; }
<<<<<<< HEAD

        //[JsonIgnore]
        //public int EstimatedReadtTime { get; set{ wordCount/60}; }
=======
>>>>>>> main
    }
}
