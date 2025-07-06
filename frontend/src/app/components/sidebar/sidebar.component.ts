import { Component, Input, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { LoginResponse } from '../../Models/Authentication/login-response';

@Component({
    selector: 'app-sidebar',
    standalone: false,
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
    @Input() isCollapsed = false;
    isLoggedIn = false;
    loggedInUser: boolean = false;
    @Input() toggleSidebar!: () => void;

    constructor(private authService: AuthenticationService) { }

    ngOnInit(): void {
    }
}

