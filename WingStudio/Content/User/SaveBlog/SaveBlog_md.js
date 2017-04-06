var mdEditor;
$(document).ready(function () {
    mdEditor = editormd("blog-editormd", {
        width: "100%",
        height: 400,
        path: "/Scripts/editor.md/lib/",
        emoji:true,
        watch: false,//关闭实时预览
        imageUpload: true,
        imageFormats: ["jpg", "jpeg", "gif", "png"],
        imageUploadURL: "/User/UploadBlogPicture",
        toolbarIcons: function () {
            return [
            "undo", "redo", "|",
            "bold", "del", "italic", "quote", "ucwords", "uppercase", "lowercase", "|",
            "list-ul", "list-ol", "hr", "|",
            "link", "reference-link", "image", "code-block", "table", "datetime", "emoji", "html-entities", "pagebreak", "|",
            "goto-line", "watch", "preview", '|',
            "help"
            ];
        }
    });
});
