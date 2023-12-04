import MarkdownIt from "markdown-it";
import hljs from "highlight.js";
import { generateUUID } from "./index";
import MarkdownItSubscript from "markdown-it-sub";
import MarkdownItSuperscript from "markdown-it-sup";
import MarkdownItMark from "markdown-it-mark";
import MarkdownItAbbreviation from "markdown-it-abbr";
import MarkdownItContainer from "markdown-it-container";
import MarkdownItDeflist from "markdown-it-deflist";
import MarkdownItEmoji from "markdown-it-emoji";
import MarkdownItFootnote from "markdown-it-footnote";
import MarkdownItInsert from "markdown-it-ins";
import MarkdownItTasklists from "markdown-it-task-lists";
// import MarkdownItKatex from "markdown-it-katex-external";
export default function markdownToHtml(content: string): string {
  const md = new MarkdownIt({
    html: true,
    linkify: true,
    typographer: true,
    breaks: true,
    highlight(str: string, lang: string = "C#"): string {
      // 生成唯一的id标识
      const codeIndex = generateUUID();
      // 复制功能主要使用的是 clipboard.js
      let html: string = `<button class="copy-btn iconfont iconfuzhi" type="button" data-clipboard-action="copy" data-clipboard-target="#copy${codeIndex}"></button>`;
      const linesLength: number = str.split(/\n/).length - 1;
      // 生成行号
      let linesNum: string =
        '<span aria-hidden="true" class="line-numbers-rows">';
      for (let index = 0; index < linesLength; index++) {
        linesNum = linesNum + "<span></span>";
      }
      linesNum += "</span>";
      if (lang && hljs.getLanguage(lang)) {
        // highlight.js 高亮代码
        const preCode = hljs.highlight(lang, str, true).value;
        html = html + preCode;
        if (linesLength) {
          html += '<b class="name">' + lang + "</b>";
        }
        // 将代码包裹在 textarea 中，由于防止textarea渲染出现问题，这里将 "<" 用 "<" 代替，不影响复制功能
        return `<pre class="hljs"><code>${html}</code>${linesNum}</pre><textarea style="position: absolute;top: -9999px;left: -9999px;z-index: -9999;" id="copy${codeIndex}">${str.replace(
          /<\/textarea>/g,
          "</textarea>"
        )}</textarea>`;
      }
      return content;
    },
  })
    .use(MarkdownItSubscript)
    .use(MarkdownItSuperscript)
    .use(MarkdownItMark)
    .use(MarkdownItAbbreviation)
    .use(MarkdownItContainer)
    .use(MarkdownItDeflist)
    .use(MarkdownItEmoji)
    .use(MarkdownItFootnote)
    .use(MarkdownItInsert)
    // .use(MarkdownItKatex)
    .use(MarkdownItTasklists);
  // 将markdown替换为html标签
  let s = md.render(content);
  if (s.length > 0) {
    s.match(/<h\d>.*?<\/h\d>/g)?.forEach((item, index) => {
      s = s.replace(item, item.replace(">", ` id="h_index_${index}">`));
    });
  }
  return s;
}
