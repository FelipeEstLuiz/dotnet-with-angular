import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AlertMessage, AlertService } from '../../_services/alert.service';

@Component({
  selector: 'app-alert',
  imports: [CommonModule],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css'
})
export class AlertComponent implements OnInit {
  
  alert: AlertMessage | null = null;

  constructor(private alertService: AlertService) {}

  ngOnInit(): void {
    this.alertService.getAlert().subscribe(alert => {
      this.alert = alert;
    });
  }

  close(): void {
    this.alert = null;
  }
}
