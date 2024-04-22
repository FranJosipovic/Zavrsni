import { Component, OnInit } from '@angular/core';
import {
  ProjectWikiData,
  ProjectWikiNode,
  WikiService,
} from './services/wiki.service';
import { FlatTreeControl, NestedTreeControl } from '@angular/cdk/tree';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
  MatTreeNestedDataSource,
} from '@angular/material/tree';
import { ContentObserver } from '@angular/cdk/observers';
import { ToastrService } from 'ngx-toastr';
import {
  CreateWikiData,
  UpdateWikiData,
} from './create-update-wiki/create-update-wiki.component';
import { UserStore } from '../../../../../store/user.store';

@Component({
  selector: 'app-wiki',
  templateUrl: './wiki.component.html',
  styleUrl: './wiki.component.scss',
})
export class WikiComponent implements OnInit {
  private projectId: string;
  public updateWikiData: UpdateWikiData | null = null;
  public createWikiData: CreateWikiData | null = null;

  treeControl = new NestedTreeControl<ProjectWikiNode>(node => node.children);
  dataSource = new MatTreeNestedDataSource<ProjectWikiNode>();

  constructor(
    private wikiService: WikiService,
    private toastr: ToastrService,
    private userStore: UserStore
  ) {
    this.projectId = localStorage.getItem('projectId')!;
  }

  public selectedNodeId: string | undefined = '';

  ngOnInit(): void {
    this.loadWikis()
  }

  loadWikis(){
    this.wikiService.getWikis(this.projectId).subscribe((data) => {
      console.log(data);
      if (data.isSuccess) {
        if (data.data!.count > 0) {
          this.selectedNodeId = data.data?.items[0]?.id;
          this.dataSource.data = data.data?.items!;
          if (this.selectedNodeId && this.selectedNodeId.length > 0) {
            this.loadItemData(this.selectedNodeId);
          }
        }
      }
    });
  }

  hasChild = (_: number, node: ProjectWikiNode) => !!node.children && node.children.length > 0;

  onNodeItemClick(id: string) {
    this.selectedNodeId = id;
    this.isEditingOrCreating = false;
    this.loadItemData(id);
  }

  loadItemData(id: string) {
    this.wikiService.getWikiData(id).subscribe((data) => {
      console.log(data);
      this.selectedProjectWiki = data.data;
    });
  }

  public selectedProjectWiki: ProjectWikiData | null | undefined;

  public isEditingOrCreating: boolean = false;

  edit() {
    this.updateWikiData = this.selectedProjectWiki
      ? {
          wikiId: this.selectedProjectWiki.id,
          title: this.selectedProjectWiki.title,
          content: this.selectedProjectWiki.content,
        }
      : null;

    this.createWikiData = this.selectedProjectWiki
      ? null
      : {
          projectId: this.projectId,
          parentId: null,
          content: '',
          createdById: this.userStore.getState().user?.id!,
        };

    console.log(this.updateWikiData);
    this.isEditingOrCreating = true;
  }

  finishCreateOrEdit() {
    this.updateWikiData = null;
    this.createWikiData = null;
    this.isEditingOrCreating = false;
    this.loadWikis()
  }
}
