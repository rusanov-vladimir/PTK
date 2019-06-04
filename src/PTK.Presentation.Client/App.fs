module App

open Bolero
open Bolero.Html

let layout (content: Node list) (customScripts:Node list) (isAdmin:bool) (categories: Node) =
    html [attr.``class`` "no-js"; attr.lang "en"] [
        head [] [
            meta [attr.charset "utf-8"]
            meta [attr.name "viewport"; attr.content "width=device-width, initial-scale=1, maximum-scale=1" ]
            title [] [text "PTK. Keeping my thoughts organized"]
            meta [attr.name "description"; attr.content "Personal tool kit"]
            meta [attr.name "author"; attr.content "3615"]

            link [attr.rel "stylesheet"; attr.href "/css/default.css" ]
            link [attr.rel "stylesheet"; attr.href "/css/layout.css" ]
            link [attr.rel "stylesheet"; attr.href "/css/media-queries.css" ]

            link [attr.rel "stylesheet"; attr.href "https://cdn.jsdelivr.net/simplemde/latest/simplemde.min.css" ]

            script [attr.src "/js/modernizr.js"] []

            link [attr.rel "shortcut icon"; attr.href "/favicon.png"]
        ]
        body [] [
            yield header [ attr.id "top" ] [
                div [attr.``class`` "row"] [
                    div [attr.``class`` "header-content twelve columns"] [
                        h1 [attr.id "logo-text"] [
                            a [attr.href "/"; attr.title "Developed with <3"] [
                                text "Welcome to personal tool kit!"
                            ]
                        ]
                        p [attr.id "intro"] [
                            text "Place where I store my stuff..."
                        ]
                    ]
                ]
                nav [attr.id "nav-wrap"] [
                    a [attr.``class`` "mobile-btn"; attr.href "#nav-wrap"; attr.title "Show navigation"][text "Show Menu" ]
                    a [attr.``class`` "mobile-btn"; attr.href "#n"; attr.title "Hide navigation"][text "Hide Menu" ]
                    div [attr.``class`` "row"] [
                        ul [attr.id "nav"; attr.``class`` "nav"][
                            yield li [attr.``class`` "current"] [
                                a [attr.href "/"] [text "Home"]
                            ]
                            yield li [] [
                                a [attr.href "/memories"] [text "Memories"]
                            ]
                            yield li [] [
                                a [attr.href "/tools"] [text "Tool kit"]
                            ]
                            yield!
                                if isAdmin 
                                    then [
                                            li [] [
                                                a [attr.href "/mems"] [text "Manage Memories"]
                                            ]
                                            li [] [
                                                a [attr.href "/cats"] [text "Manage Categories"]
                                            ]
                                        ]
                                    else []
                            yield li [] [
                                a [attr.href "/about"] [text "About"]
                            ]
                            yield li [] [
                                a [attr.href "/contacts"] [text "Contacts"]
                            ]
                        ]
                    ]
                ]
            ]
            yield div [attr.id "content-wrap"][
                div [attr.``class`` "row"] [
                    div [attr.id "main"; attr.``class`` "eight columns"] [
                        yield! content
                    ]
                    div [attr.id "sidebar"; attr.``class`` "four columns"][
                        div [attr.``class`` "widget widgetattr.search"][                        
                            h3 [][text "Search"]
                            form [attr.action "/search"; attr.method "GET"][
                                input [attr.``type`` "text"; attr.name "searchString"; attr.placeholder "Search here..."; attr.``class`` "text-search"]
                                input [attr.``type`` "submit"; attr.value ""; attr.``class`` "submit-search"]
                            ]
                        ]
                        div [attr.``class`` "widget widgetattr.categories group"][
                            categories
                        ]
                        div [attr.``class`` "widget widgetattr.tags"][
                            //todo: Get real tags
                            h3 [][text "Post Tags."]
                            div [attr.``class`` "tagcloud group"][
                                a [attr.href "#"][text "Corporate"]
                                a [attr.href "#"][text "Onepage"]
                                a [attr.href "#"][text "Agency"]
                                a [attr.href "#"][text "Multipurpose"]
                                a [attr.href "#"][text "Blog"]
                                a [attr.href "#"][text "Landing Page"]
                                a [attr.href "#"][text "Resume"]
                            ]
                        ]
                        div [attr.``class`` "widget widgetattr.popular"] [
                            //todo: Get real posts
                            h3 [][text "Popular Post."]
                            ul [attr.``class`` "link-list"][
                                li [][
                                    a [attr.href "#"][text "Sint cillum consectetur voluptate."] 
                                ]
                                li [][
                                    a [attr.href "#"][text "Lorem ipsum Ullamco commodo."] 
                                ]
                                li [][
                                    a [attr.href "#"][text "Fugiat minim eiusmod do."] 
                                ]
                            ]
                        ]
                    ] // end sidebar
                ] // end row
            ] // end content-wrap
            
            yield footer [] [
                div [attr.``class`` "row"] [
 
                    div [attr.``class`` "six columns info"][
                        h3 [][text "About Personal Tool Kit"]
                        p [][text 
                                "This site is born out of curiosity for functional programming and F# particulary. Now it's a home for casual notes on various topics."
                            ]
                        p [][text 
                                "It's free and opensource, so everybody can inspire from it checking out Github repo. Thanks for Heroku free hosting and Travis CI for being able to make it public."
                            ]
                    ]
                    div [attr.``class`` "four columns"][
                        h3 [][text "Social links"]
                        ul [attr.``class``"social-links photostream group"][
                               li [][a [attr.href "https://www.facebook.com/vladimir.rusanov.5"][i [attr.``class`` "fa fa-facebook"] []]]
                               li [][a [attr.href "https://twitter.com/3615us"][i [attr.``class`` "fa fa-twitter"] []]]
                               li [][a [attr.href "https://github.com/rusanov-vladimir"][i [attr.``class`` "fa fa-github-square"] []]]
                               li [][a [attr.href "https://vk.com/id6559326"][i [attr.``class`` "fa fa-vk"] []]]
                        ]
                    ]
                    div [attr.``class`` "two columns"][
                        h3 [attr.``class`` "social"][text "Navigate"]
                        ul [attr.``class`` "navigate group"][
                            li [][a [attr.href "/"][text "Home"]]
                            li [][a [attr.href "/memories"][text "Memories"]]
                            li [][a [attr.href "/tools"][text "Tools"]]
                            li [][a [attr.href "/contacts"][text "Contacts"]]
                            li [][a [attr.href "/about"][text "About"]]
                            li [][a [attr.href "/admin"][text "Admin"]]
                        ]
                    ]
                    p [attr.``class`` "copyright"][text """&copy; Copyright 2014 Personal Tool Kit. &nbsp;"""]
                ] //end row
                div [attr.id "go-top"][
                    a [attr.``class`` "smoothscroll"; attr.title "Back to Top"; attr.href "#top"][
                        i [attr.``class`` "fa fa-chevron-up"] []
                    ]
                ]
            ] //end footer

            yield script [attr.``type`` "text/javascript"; attr.src "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"] []
            yield script [attr.``type`` "text/javascript"; attr.src "https://unpkg.com/infinite-scroll@3/dist/infinite-scroll.pkgd.min.js"] []
            yield script [attr.``type`` "text/javascript"; attr.src "https://cdn.jsdelivr.net/simplemde/latest/simplemde.min.js"] []
            yield script [] [text """window.jQuery || document.write('<script src="js/jquery-1.10.2.min.js"><\/script>')"""]
            yield script [attr.``type`` "text/javascript"; attr.src "/js/jquery-migrate-1.2.1.min.js"] []
            yield script [attr.``type`` "text/javascript"; attr.src "/js/main.js"] []
            yield script [attr.src "/app.js"] []
            yield! customScripts
        ]
    ]
