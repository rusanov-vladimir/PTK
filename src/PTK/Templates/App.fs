module App

open Giraffe.GiraffeViewEngine

let layout (content: XmlNode list) (customScripts:XmlNode list) (isAdmin:bool) (categories: XmlNode) =
    html [_class "no-js"; _lang "en"] [
        head [] [
            meta [_charset "utf-8"]
            meta [_name "viewport"; _content "width=device-width, initial-scale=1, maximum-scale=1" ]
            title [] [encodedText "PTK. Keeping my thoughts organized"]
            meta [_name "description"; _content "Personal tool kit"]
            meta [_name "author"; _content "3615"]

            link [_rel "stylesheet"; _href "/css/default.css" ]
            link [_rel "stylesheet"; _href "/css/layout.css" ]
            link [_rel "stylesheet"; _href "/css/media-queries.css" ]

            link [_rel "stylesheet"; _href "https://cdn.jsdelivr.net/simplemde/latest/simplemde.min.css" ]

            script [_src "/js/modernizr.js"] []

            link [_rel "shortcut icon"; _href "/favicon.png"]
        ]
        body [] [
            yield header [ _id "top" ] [
                div [_class "row"] [
                    div [_class "header-content twelve columns"] [
                        h1 [_id "logo-text"] [
                            a [_href "/"; _title "Developed with <3"] [
                                encodedText "Welcome to personal tool kit!"
                            ]
                        ]
                        p [_id "intro"] [
                            encodedText "Place where I store my stuff..."
                        ]
                    ]
                ]
                nav [_id "nav-wrap"] [
                    a [_class "mobile-btn"; _href "#nav-wrap"; _title "Show navigation"][encodedText "Show Menu" ]
                    a [_class "mobile-btn"; _href "#n"; _title "Hide navigation"][encodedText "Hide Menu" ]
                    div [_class "row"] [
                        ul [_id "nav"; _class "nav"][
                            yield li [_class "current"] [
                                a [_href "/"] [encodedText "Home"]
                            ]
                            yield li [] [
                                a [_href "/memories"] [encodedText "Memories"]
                            ]
                            yield li [] [
                                a [_href "/tools"] [encodedText "Tool kit"]
                            ]
                            yield!
                                if isAdmin 
                                    then [
                                            li [] [
                                                a [_href "/mems"] [encodedText "Manage Memories"]
                                            ]
                                            li [] [
                                                a [_href "/cats"] [encodedText "Manage Categories"]
                                            ]
                                        ]
                                    else []
                            yield li [] [
                                a [_href "/about"] [encodedText "About"]
                            ]
                            yield li [] [
                                a [_href "/contacts"] [encodedText "Contacts"]
                            ]
                        ]
                    ]
                ]
            ]
            yield div [_id "content-wrap"][
                div [_class "row"] [
                    div [_id "main"; _class "eight columns"] [
                        yield! content
                    ]
                    div [_id "sidebar"; _class "four columns"][
                        div [_class "widget widget_search"][                        
                            h3 [][encodedText "Search"]
                            form [_action "/search"; _method "GET"][
                                input [_type "text"; _name "searchString"; _placeholder "Search here..."; _class "text-search"]
                                input [_type "submit"; _value ""; _class "submit-search"]
                            ]
                        ]
                        div [_class "widget widget_categories group"][
                            categories
                        ]
                        div [_class "widget widget_tags"][
                            //todo: Get real tags
                            h3 [][encodedText "Post Tags."]
                            div [_class "tagcloud group"][
                                a [_href "#"][encodedText "Corporate"]
                                a [_href "#"][encodedText "Onepage"]
                                a [_href "#"][encodedText "Agency"]
                                a [_href "#"][encodedText "Multipurpose"]
                                a [_href "#"][encodedText "Blog"]
                                a [_href "#"][encodedText "Landing Page"]
                                a [_href "#"][encodedText "Resume"]
                            ]
                        ]
                        div [_class "widget widget_popular"] [
                            //todo: Get real posts
                            h3 [][encodedText "Popular Post."]
                            ul [_class "link-list"][
                                li [][
                                    a [_href "#"][encodedText "Sint cillum consectetur voluptate."] 
                                ]
                                li [][
                                    a [_href "#"][encodedText "Lorem ipsum Ullamco commodo."] 
                                ]
                                li [][
                                    a [_href "#"][encodedText "Fugiat minim eiusmod do."] 
                                ]
                            ]
                        ]
                    ] // end sidebar
                ] // end row
            ] // end content-wrap
            
            yield footer [] [
                div [_class "row"] [
 
                    div [_class "six columns info"][
                        h3 [][encodedText "About Personal Tool Kit"]
                        p [][encodedText 
                                "This site is born out of curiosity for functional programming and F# particulary. Now it's a home for casual notes on various topics."
                            ]
                        p [][encodedText 
                                "It's free and opensource, so everybody can inspire from it checking out Github repo. Thanks for Heroku free hosting and Travis CI for being able to make it public."
                            ]
                    ]
                    div [_class "four columns"][
                        h3 [][encodedText "Social links"]
                        ul [_class"social-links photostream group"][
                               li [][a [_href "https://www.facebook.com/vladimir.rusanov.5"][i [_class "fa fa-facebook"] []]]
                               li [][a [_href "https://twitter.com/3615us"][i [_class "fa fa-twitter"] []]]
                               li [][a [_href "https://github.com/rusanov-vladimir"][i [_class "fa fa-github-square"] []]]
                               li [][a [_href "https://vk.com/id6559326"][i [_class "fa fa-vk"] []]]
                        ]
                    ]
                    div [_class "two columns"][
                        h3 [_class "social"][encodedText "Navigate"]
                        ul [_class "navigate group"][
                            li [][a [_href "/"][encodedText "Home"]]
                            li [][a [_href "/memories"][encodedText "Memories"]]
                            li [][a [_href "/tools"][encodedText "Tools"]]
                            li [][a [_href "/contacts"][encodedText "Contacts"]]
                            li [][a [_href "/about"][encodedText "About"]]
                            li [][a [_href "/admin"][encodedText "Admin"]]
                        ]
                    ]
                    p [_class "copyright"][rawText """&copy; Copyright 2014 Personal Tool Kit. &nbsp;"""]
                ] //end row
                div [_id "go-top"][
                    a [_class "smoothscroll"; _title "Back to Top"; _href "#top"][
                        i [_class "fa fa-chevron-up"] []
                    ]
                ]
            ] //end footer

            yield script [_type "text/javascript"; _src "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"] []
            yield script [_type "text/javascript"; _src "https://unpkg.com/infinite-scroll@3/dist/infinite-scroll.pkgd.min.js"] []
            yield script [_type "text/javascript"; _src "https://cdn.jsdelivr.net/simplemde/latest/simplemde.min.js"] []
            yield script [] [rawText """window.jQuery || document.write('<script src="js/jquery-1.10.2.min.js"><\/script>')"""]
            yield script [_type "text/javascript"; _src "/js/jquery-migrate-1.2.1.min.js"] []
            yield script [_type "text/javascript"; _src "/js/main.js"] []
            yield script [_src "/app.js"] []
            yield! customScripts
        ]
    ]
