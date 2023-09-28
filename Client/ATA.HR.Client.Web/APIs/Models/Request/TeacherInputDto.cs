﻿namespace ATA.HR.Client.Web.APIs.Models.Request;

public class TeacherInputDto
{
    public string SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}