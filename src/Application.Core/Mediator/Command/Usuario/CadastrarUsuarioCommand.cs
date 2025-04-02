﻿using Application.Domain.Model;
using MediatR;

namespace Application.Core.Mediator.Command.Usuario;

public record CadastrarUsuarioCommand : IRequest<Result<bool>>
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string SenhaConfirmacao { get; set; } = null!;
}
