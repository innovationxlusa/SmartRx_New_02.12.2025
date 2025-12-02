import React from "react";
import BlogCard from "./BlogCard";
import { blogPosts } from "./blogData";
import "./AdviceBlog.css";
import PageTitle from "../../static/PageTitle/PageTitle";
import { useLocation } from "react-router-dom";

const AdviceBlog = () => {
    const location = useLocation();
    const question = location.state?.question;
    return (
      <div className="main-container">
        <section className="hero">
          <PageTitle
            switchButton={false}
            pageName={question}
          />
          <div className="d-flex justify-content-center mt-2">
            <p>
              Explore dental tips, health trends, and expert advice —
              beautifully curated just for you.
            </p>
          </div>
        </section>
        <section className="blog-grid">
          {blogPosts.map((post) => (
            <BlogCard key={post.id} post={post} />
          ))}
        </section>
      </div>
    );
};

export default AdviceBlog;
