import { Component, OnInit } from '@angular/core';
import { UserStore } from '../../../../store/user.store';
import { User } from '../../../../core/user/interfaces';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss',
})
export class ProjectComponent implements OnInit {
  public userState: User;

  public menu = [
    {
      title: 'Overview',
      isSelected: true,
      children: [
        { title: 'Summary', isSelected: true },
        { title: 'Wiki', isSelected: false },
      ],
    },
    {
      title: 'Boards',
      isSelected: false,
      children: [
        { title: 'Work Items', isSelected: false },
        { title: 'Sprints', isSelected: false },
      ],
    },
    {
      title: 'Repos',
      isSelected: false,
      children: [
        { title: 'Files', isSelected: false },
        { title: 'Commits', isSelected: false },
      ],
    },
  ];

  onSelectParentItemMenu(parentTitle: string) {
    this.menu.forEach((menuItem) => {
      if (menuItem.title === parentTitle) {
        menuItem.isSelected = true;
        if (menuItem.children && menuItem.children.length > 0) {
          menuItem.children.forEach((child, index) => {
            child.isSelected = index === 0;
          });
        }
      } else {
        menuItem.isSelected = false;
        if (menuItem.children) {
          menuItem.children.forEach((child) => {
            child.isSelected = false;
          });
        }
      }
    });
  }

  onSelectChildItemMenu(parentTitle:string,childTitle:string){
    this.menu.forEach(parent=>{
      if(parent.title === parentTitle){
        parent.children.forEach(child=>{
          if(child.title === childTitle){
            child.isSelected = true
          }else{
            child.isSelected = false
          }
        })
      }
    })
  }

  constructor(private userStore: UserStore, private route: ActivatedRoute) {
    this.userState = userStore.getState().user!;
  }
  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      console.log(data);
    });
  }
}
