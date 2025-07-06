import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  avatar: string;
}

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {
  users: User[] = [
  {
    id: 1,
    name: 'Alice Johnson',
    email: 'alice.johnson23@gmail.com',
    role: 'Admin',
    avatar: 'https://randomuser.me/api/portraits/women/1.jpg'
  },
  {
    id: 2,
    name: 'Bob Smith',
    email: 'bobsmith1985@outlook.com',
    role: 'User',
    avatar: 'https://randomuser.me/api/portraits/men/2.jpg'
  },
  {
    id: 3,
    name: 'Carol Lee',
    email: 'carol.lee92@gmail.com',
    role: 'Moderator',
    avatar: 'https://randomuser.me/api/portraits/women/3.jpg'
  },
  {
    id: 4,
    name: 'David Kim',
    email: 'david.kim01@outlook.com',
    role: 'User',
    avatar: 'https://randomuser.me/api/portraits/men/4.jpg'
  },
  {
    id: 5,
    name: 'Eva Green',
    email: 'evagreen77@gmail.com',
    role: 'Admin',
    avatar: 'https://randomuser.me/api/portraits/women/5.jpg'
  },
  {
    id: 6,
    name: 'Frank Miller',
    email: 'frank.miller84@outlook.com',
    role: 'User',
    avatar: 'https://randomuser.me/api/portraits/men/6.jpg'
  },
  {
    id: 7,
    name: 'Grace Lee',
    email: 'grace.lee09@gmail.com',
    role: 'Moderator',
    avatar: 'https://randomuser.me/api/portraits/women/7.jpg'
  },
  {
    id: 8,
    name: 'Henry Ford',
    email: 'henry.ford2020@gmail.com',
    role: 'User',
    avatar: 'https://randomuser.me/api/portraits/men/8.jpg'
  },
  {
    id: 9,
    name: 'Ivy Chen',
    email: 'ivy.chen13@outlook.com',
    role: 'Admin',
    avatar: 'https://randomuser.me/api/portraits/women/9.jpg'
  }
];

}
