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

            link [_rel "stylesheet"; _href "css/default.css" ]
            link [_rel "stylesheet"; _href "css/layout.css" ]
            link [_rel "stylesheet"; _href "css/media-queries.css" ]

            script [_src "js/modernizr.js"] []

            link [_rel "shortcut icon"; _href "favicon.png"]
        ]
        body [] [
            yield header [ _id "top" ] [
                div [_class "row"] [
                    div [_class "header-content twelve columns"] [
                        h1 [_id "logo-text"] [
                            a [_href "/"; _title "ad"] [
                                rawText "Keep It Simple."
                            ]
                        ]
                        p [_id "intro"] [
                            rawText "Put your awesome slogan here..."
                        ]
                    ]
                ]
                nav [_id "nav-wrap"] [
                    a [_class "mobile-btn"; _href "#nav-wrap"; _title "Show navigation"][rawText "Show Menu" ]
                    a [_class "mobile-btn"; _href "#n"; _title "Hide navigation"][rawText "Hide Menu" ]
                    div [_class "row"] [
                        ul [_id "nav"; _class "nav"][
                            li [_class "current"] [
                                a [_href "/"] [rawText "Home"]
                            ]
                            li [] [
                                a [_href "/mems"] [rawText "Memories3615"]
                            ]
                            li [] [
                                a [_href "/tools"] [rawText "Tool kit"]
                            ]
                            li [] [
                                a [_href "/contacts"] [rawText "Contacts"]
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
                            h3 [][rawText "Search"]
                            form [_action "#"][
                                input [_type "text"; _value "Search here..."; _class "text-search"]
                                input [_type "submit"; _value ""; _class "submit-search"]
                            ]
                        ]
                        div [_class "widget widget_categories group"][
                            //todo: Get real categories
                            h3 [][rawText "Categories."]
                            ul [][
                                li [][
                                    a [_href "#"][rawText "Wordpress"] 
                                    rawText "(2)"
                                ]
                                li [][
                                    a [_href "#"][rawText "Ghost"] 
                                    rawText "(14)"
                                ]
                                li [][
                                    a [_href "#"][rawText "Joomla"] 
                                    rawText "(5)"
                                ]
                                li [][
                                    a [_href "#"][rawText "Drupal"] 
                                    rawText "(3)"
                                ]
                                li [][
                                    a [_href "#"][rawText "Magento"] 
                                    rawText "(2)"
                                ]
                                li [][
                                    a [_href "#"][rawText "Uncategorized"] 
                                    rawText "(9)"
                                ]
                            ]
                        ]
                        div [_class "widget widget_tags"][
                            //todo: Get real tags
                            h3 [][rawText "Post Tags."]
                            div [_class "tagcloud group"][
                                a [_href "#"][rawText "Corporate"]
                                a [_href "#"][rawText "Onepage"]
                                a [_href "#"][rawText "Agency"]
                                a [_href "#"][rawText "Multipurpose"]
                                a [_href "#"][rawText "Blog"]
                                a [_href "#"][rawText "Landing Page"]
                                a [_href "#"][rawText "Resume"]
                            ]
                        ]
                        div [_class "widget widget_popular"] [
                            //todo: Get real posts
                            h3 [][rawText "Popular Post."]
                            ul [_class "link-list"][
                                li [][
                                    a [_href "#"][rawText "Sint cillum consectetur voluptate."] 
                                ]
                                li [][
                                    a [_href "#"][rawText "Lorem ipsum Ullamco commodo."] 
                                ]
                                li [][
                                    a [_href "#"][rawText "Fugiat minim eiusmod do."] 
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
                        h3 [][rawText "About Keep It Simple"]
                        p [][rawText 
                                "This is Photoshop's version  of Lorem Ipsum. Proin gravida nibh vel velit auctor aliquet. Aenean sollicitudin, lorem quis bibendum auctor, nisi elit consequat ipsum, nec sagittis sem nibh id elit. "
                            ]
                        p [][rawText 
                                "Lorem ipsum Sed nulla deserunt voluptate elit occaecat culpa cupidatat sit irure sint sint incididunt cupidatat esse in Ut sed commodo tempor consequat culpa fugiat incididunt."
                            ]
                    ]
                    div [_class "four columns"][
                        h3 [][rawText "Photostream"]
                        ul [_class"photostream group"][
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                            li [][a [_href "#"][img [_alt "thumbnail"; _src "images/thumb.jpg"] ]]
                        ]
                    ]
                    div [_class "two columns"][
                        h3 [_class "social"][rawText "Navigate"]
                        ul [_class "navigate group"][
                            li [][a [_href "#"][rawText "Home"]]
                            li [][a [_href "#"][rawText "Blog"]]
                            li [][a [_href "#"][rawText "Demo"]]
                            li [][a [_href "#"][rawText "Archives"]]
                            li [][a [_href "#"][rawText "About"]]
                        ]
                    ]
                    p [_class "copyright"][rawText """&copy; Copyright 2014 Keep It Simple. &nbsp; Design by <a title="3615" href="#">3615</a>."""]
                ] //end row
                div [_id "go-top"][
                    a [_class "smoothscroll"; _title "Back to Top"; _href "#top"][
                        i [_class "fa fa-chevron-up"] []
                    ]
                ]
            ] //end footer

            yield script [_type "text/javascript"; _src "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"] []            
            yield script [] [rawText """window.jQuery || document.write('<script src="js/jquery-1.10.2.min.js"><\/script>')"""]
            yield script [_type "text/javascript"; _src "js/jquery-migrate-1.2.1.min.js"] []
            yield script [_type "text/javascript"; _src "js/main.js"] []
        ]
    ]
