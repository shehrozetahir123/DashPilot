import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {


  constructor(private authService: AuthenticationService) { }

  recentActivities = [
    { date: '2025-06-29', description: 'User Ahmed registered', status: 'Success' },
    { date: '2025-06-28', description: 'Payment failed for Order #1234', status: 'Failed' },
    { date: '2025-06-27', description: 'Report generated', status: 'Success' },
  ];

  revenueLabels = ['Jan', 'Feb', 'Mar', 'Apr', 'May'];
  revenueData = [{ data: [5000, 10000, 7500, 15000, 20000], label: 'Revenue' }];

  usersLabels = ['Jan', 'Feb', 'Mar', 'Apr', 'May'];
  usersData = [{ data: [50, 120, 180, 300, 450], label: 'New Users' }];

  chartOptions = {
    responsive: true,
    // maintainAspectRatio: false,
  };
}
