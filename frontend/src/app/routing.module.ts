import { NgModule } from "@angular/core";
import {  RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./login/login/login.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { AuthGuard } from "./guards/auth-guard.guard";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { SignUpComponent } from "./login/sign-up/sign-up.component";
import { UsersComponent } from "./components/users/users.component";

const routes: Routes = [
    {path:'', redirectTo: 'login', pathMatch: 'full'},
    {path:'login', component: LoginComponent},
    {path:'signup', component: SignUpComponent},
    {path:'dashboard', component: DashboardComponent, canActivate:[AuthGuard]},
    {path:'users', component: UsersComponent, canActivate:[AuthGuard]},
    { path: '**', component: NotFoundComponent }
]

@NgModule({
    imports:[
        RouterModule.forRoot(routes)
    ],
    exports:[
        RouterModule
    ]
})
export class RoutingModule{

}