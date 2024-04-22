import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { WikiComponent } from '../wiki.component';
import { WikiService } from '../services/wiki.service';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ToastrService } from 'ngx-toastr';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-update-wiki',
  templateUrl: './create-update-wiki.component.html',
  styleUrl: './create-update-wiki.component.scss',
})
export class CreateUpdateWikiComponent implements OnInit {
  @Input() updateWikiData!: UpdateWikiData | null;
  @Input() createWikiData!: CreateWikiData | null;

  @Output() finishEditOrCreate = new EventEmitter();

  public title = new FormControl('', Validators.required);

  finishAction() {
    this.finishEditOrCreate.emit();
  }

  public Editor = ClassicEditor;
  private editedContent: string = '';
  public isEditing!: boolean;

  public onChange({ editor }: ChangeEvent) {
    this.editedContent = editor.getData();
  }

  constructor(
    private wikiService: WikiService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.isEditing =
      this.updateWikiData !== null && this.updateWikiData !== undefined;
    console.log(this.isEditing);
  }

  close() {
    this.finishAction();
  }

  save() {
    this.wikiService
      .updateWiki(this.updateWikiData?.wikiId!, this.editedContent)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-success'
          );
        } else {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-error'
          );
        }
        this.editedContent = '';
        this.finishAction();
      });
  }

  create() {
    console.log(this.title)
    this.wikiService
      .createWiki(
        this.editedContent,
        this.title.value!,
        this.createWikiData?.projectId!,
        this.createWikiData?.createdById!,
        this.createWikiData?.parentId!
      )
      .subscribe((data) => {
        if (data.isSuccess) {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-success'
          );
        } else {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-error'
          );
        }
        this.editedContent = '';
        this.finishAction();
      });
  }
}

export interface UpdateWikiData {
  wikiId: string;
  title: string;
  content: string;
}

export interface CreateWikiData {
  projectId: string;
  parentId: string | null;
  content: string;
  createdById: string;
}
