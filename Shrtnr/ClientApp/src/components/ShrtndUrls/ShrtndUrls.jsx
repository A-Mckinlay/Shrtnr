import React from "react";
import { ListGroup, ListGroupItem } from "reactstrap";
import "./ShrtndUrls.css"

export const ShrtndUrls = (props) => (
    <ListGroup className="shrtnd-urls-ul">
        { props.urls.map(urlPair => 
            <ShrtndUrlLi 
                url={urlPair.url}
                code={urlPair.code}
                key={urlPair.code}
            />
        )}
    </ListGroup>
)

const ShrtndUrlLi = (props) => {
    const shrtUrl = `${window.location.origin}/${props.code}`;
    
    return (
        <ListGroupItem className="shrtnd-url-li">
            <span>{props.url}</span>
            <span><a href={shrtUrl}>{shrtUrl}</a></span>
        </ListGroupItem>
    );
}

export default ShrtndUrls