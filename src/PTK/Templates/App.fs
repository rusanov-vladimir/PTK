module App

open Giraffe.GiraffeViewEngine

let layout (content: XmlNode list) =
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
                            li [_class "current"] [
                                a [_href "/"] [encodedText "Home"]
                            ]
                            li [] [
                                a [_href "/memories"] [encodedText "Memories3615"]
                            ]
                            li [] [
                                a [_href "/tools"] [encodedText "Tool kit"]
                            ]
                            li [] [
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
                            form [_action "#"][
                                input [_type "text"; _value "Search here..."; _class "text-search"]
                                input [_type "submit"; _value ""; _class "submit-search"]
                            ]
                        ]
                        div [_class "widget widget_categories group"][
                            //todo: Get real categories
                            h3 [][encodedText "Categories."]
                            ul [][
                                li [][
                                    a [_href "#"][encodedText "Wordpress"] 
                                    encodedText "(2)"
                                ]
                                li [][
                                    a [_href "#"][encodedText "Ghost"] 
                                    encodedText "(14)"
                                ]
                                li [][
                                    a [_href "#"][encodedText "Joomla"] 
                                    encodedText "(5)"
                                ]
                                li [][
                                    a [_href "#"][encodedText "Drupal"] 
                                    encodedText "(3)"
                                ]
                                li [][
                                    a [_href "#"][encodedText "Magento"] 
                                    encodedText "(2)"
                                ]
                                li [][
                                    a [_href "#"][encodedText "Uncategorized"] 
                                    encodedText "(9)"
                                ]
                            ]
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
                    div [_class "twelve columns"] [
                        ul [_class "social-links"][
                           li [][a [_href "#"][i [_class "fa fa-facebook"] []]]
                           li [][a [_href "#"][i [_class "fa fa-twitter"] []]]
                           li [][a [_href "#"][i [_class "fa fa-google-plus"] []]]
                           li [][a [_href "#"][i [_class "fa fa-github-square"] []]]
                           li [][a [_href "#"][i [_class "fa fa-instagram"] []]]
                           li [][a [_href "#"][i [_class "fa fa-flickr"] []]]
                           li [][a [_href "#"][i [_class "fa fa-skype"] []]]
                        ]
                    ]
                    div [_class "six columns info"][
                        h3 [][encodedText "About Keep It Simple"]
                        p [][encodedText 
                                "This is Photoshop's version  of Lorem Ipsum. Proin gravida nibh vel velit auctor aliquet. Aenean sollicitudin, lorem quis bibendum auctor, nisi elit consequat ipsum, nec sagittis sem nibh id elit. "
                            ]
                        p [][encodedText 
                                "Lorem ipsum Sed nulla deserunt voluptate elit occaecat culpa cupidatat sit irure sint sint incididunt cupidatat esse in Ut sed commodo tempor consequat culpa fugiat incididunt."
                            ]
                    ]
                    div [_class "four columns"][
                        h3 [][encodedText "Photostream"]
                        ul [_class"photostream group"][
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "/images/thumb.jpg"] ]]
                        ]
                    ]
                    div [_class "two columns"][
                        h3 [_class "social"][encodedText "Navigate"]
                        ul [_class "navigate group"][
                            li [][a [_href "#"][encodedText "Home"]]
                            li [][a [_href "#"][encodedText "Blog"]]
                            li [][a [_href "#"][encodedText "Demo"]]
                            li [][a [_href "#"][encodedText "Archives"]]
                            li [][a [_href "#"][encodedText "About"]]
                        ]
                    ]
                    p [_class "copyright"][rawText """&copy; Copyright 2014 Keep It Simple. &nbsp;"""]
                ] //end row
                div [_id "go-top"][
                    a [_class "smoothscroll"; _title "Back to Top"; _href "#top"][
                        i [_class "fa fa-chevron-up"] []
                    ]
                ]
            ] //end footer

            yield script [_type "text/javascript"; _src "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"] []
            yield script [_type "text/javascript"; _src "https://unpkg.com/infinite-scroll@3/dist/infinite-scroll.pkgd.min.js"] []
            yield script [] [rawText """window.jQuery || document.write('<script src="js/jquery-1.10.2.min.js"><\/script>')"""]
            yield script [_type "text/javascript"; _src "/js/jquery-migrate-1.2.1.min.js"] []
            yield script [_type "text/javascript"; _src "/js/main.js"] []
            yield script [_src "/app.js"] []
        ]
    ]
