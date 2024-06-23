﻿using Componentes.Data.Models;

namespace Componentes.Data.Repositories;

public class BaseRepository
{
    protected readonly ProyectoComponentesContext Context;

    protected BaseRepository(ProyectoComponentesContext context)
        => Context = context;
}