﻿using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;

namespace BackendServer.Models.UserModels.DTOs;

public class PublicUserDTO
{
    public string Username { get; set; }
    public int Karma { get; set; }
    public List<ProblemDTO> Problems { get; set; } 
    public List<FixDTO> Fixes { get; set; } 
    
}