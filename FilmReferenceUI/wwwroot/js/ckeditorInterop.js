window.FilmReference = {
    initCkEditor: function (id, initialValue, dotNetRef) {

        const element = document.getElementById(id);
        if (!element) {
            console.error("CKEditor element not found:", id);
            return;
        }

        CKEDITOR.ClassicEditor
            .create(element, {
                toolbar: {
                    items: [
                        'bold',
                        'italic',
                        'underline',
                        '|',
                        'fontColor',
                        'fontBackgroundColor',
                        '|',
                        'paragraph'
                    ]
                },
                placeholder: 'Notes',
                removePlugins: [
                    'CloudServices',
                    'RealTimeCollaborativeComments',
                    'RealTimeCollaborativeRevisionHistory',
                    'RealTimeCollaborativeTrackChanges',
                    'PresenceList',
                    'Comments',
                    'TrackChanges',
                    'TrackChangesData',
                    'RevisionHistory',
                    'CKBox',
                    'CKBoxImageEdit',
                    'CKFinder',
                    'CKFinderUploadAdapter',
                    'AIAssistant',
                    'ExportPdf',
                    'ExportWord',
                    'WProofreader',
                    'MathType',
                    'EasyImage',
                    'Pagination',
                    'Template',
                    'FormatPainter',
                    'DocumentOutline',
                    'TableOfContents',
                    'SlashCommand',
                    'PasteFromOfficeEnhanced',
                    'CaseChange',
                    'SourceEditing'
                ]
            })
            .then(editor => {
                editor.setData(initialValue || "");

                editor.model.document.on('change:data', () => {
                    dotNetRef.invokeMethodAsync("OnEditorChanged", editor.getData());
                });
            })
            .catch(error => console.error("CKEditor init error:", error));
    }
};
