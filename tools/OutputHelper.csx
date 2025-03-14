
#r "nuget: Markdig, 0.40.0"
using Markdig;
using Markdig.Renderers;
using System.IO;

public record class HtmlValue(string Html) {
    static HtmlValue() => Microsoft.DotNet.Interactive.Formatting.Formatter.Register<HtmlValue>(
        htmlValue => htmlValue.Html,
        mimeType: "text/html"
    );
};

public static class OutputHelper
{
    public static HtmlValue RenderMarkdownHtml(string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseMathematics()
            .UseAdvancedExtensions()
            .Build();

        var document = Markdown.Parse(markdown);

        using var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        pipeline.Setup(renderer);
        renderer.Render(document);

        return new HtmlValue(writer.ToString());
    }

    public static HtmlValue RenderHtmlForConversation(string question, string answer)
    {
        var botContent = answer is { Length: > 0 } ?
            $$"""
            <div class="message bot">
                <div class="bubble">
                    {{RenderMarkdownHtml(answer).Html}}
                </div>
            </div>
            """ :
            """     
            <div class="message bot">
                <i>Agents are collaborating to come up with an answer for you</i>
                <div class="loading-dots"><span></span><span></span><span></span></div>
            </div>
            """;   
        var chatUI =
        $$"""
        <style>
            body {
                font-family: Arial, sans-serif;
                background: #f5f5f5;
                padding: 20px;
            }
            .chat-window {
                max-width: 500px;
                margin: 0 auto;
                background: #fff;
                border: 1px solid #ddd;
                border-radius: 5px;
                padding: 15px;
            }
            .message {
                margin-bottom: 15px;
            }
            .user .bubble {
                background: #0084ff;
                color: #fff;
                text-align: right;
                border-radius: 15px 15px 0 15px;
                padding: 10px 15px;
            }
            .bot {
                background: #e5e5ea;
                color: #000;
                text-align: left;
                border-radius: 15px 15px 15px 0;
                padding: 10px 15px;
            }
            .loading-dots {
                display: inline-block;
                margin-left: 5px;
            }
            .loading-dots span {
                background-color: #666;
                display: inline-block;
                width: 6px;
                height: 6px;
                border-radius: 50%;
                margin-right: 3px;
                animation: dots 1.5s infinite;
            }
            .loading-dots span:nth-child(2) {
                animation-delay: 0.5s;
            }
            .loading-dots span:nth-child(3) {
                animation-delay: 1s;
            }
            @keyframes dots {
                0%, 100% { opacity: 0; }
                50% { opacity: 1; }
            }
        </style>
        <div class="chat-window">
            <div class="message user">
                <div class="bubble">
                    {{RenderMarkdownHtml(question).Html}}
                </div>
            </div>
            {{botContent}}
        </div>
        """;

        return new HtmlValue(chatUI);
    }
}