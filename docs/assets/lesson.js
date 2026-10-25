// 每一課共用：Mermaid 主題跟隨系統深淺色、程式碼上色
(function () {
  var dark = window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches;
  if (window.mermaid) {
    mermaid.initialize({ startOnLoad: true, theme: dark ? "dark" : "default", securityLevel: "loose" });
  }
  if (window.hljs) {
    document.addEventListener("DOMContentLoaded", function () { hljs.highlightAll(); });
  }
})();
