import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EventosComponent } from './components/eventos/eventos.component';
import { EventoDetalheComponent } from './components/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/eventos/evento-lista/evento-lista.component';

import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';

import { PerfilComponent } from './components/user/perfil/perfil.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {path: '', redirectTo: 'home', pathMatch: 'full'},
  {
    path: 'user', component: UserComponent,
    children: [
      {path: 'login', component: LoginComponent},
      {path: 'registration', component: RegistrationComponent},
    ]
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
        {path: 'eventos', redirectTo: 'eventos/lista'},
        {
          path: 'eventos', component: EventosComponent,
          children: [
            {path: 'detalhe', component: EventoDetalheComponent},
            {path: 'detalhe/:id', component: EventoDetalheComponent},
            {path: 'lista', component: EventoListaComponent},
          ]
        },
        {path: 'contatos', component: ContatosComponent},
        {path: 'dashboard', component: DashboardComponent},
        {path: 'palestrantes', component: PalestrantesComponent},
        {path: 'user', redirectTo: 'user/perfil'},
        {path: 'user/perfil', component: PerfilComponent},
      ]
    },
    {path: 'home', component: HomeComponent},
    {path: '**', redirectTo: 'home', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
