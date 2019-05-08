UPDATE business
SET review_count = (SELECT COUNT(business_id) 
				   	FROM review
				   	WHERE review.business_id = business.business_id);
					
					
UPDATE business
SET reviewrating = (SELECT AVG(review_stars) 
				   	FROM review
				   	WHERE review.business_id = business.business_id);
					
UPDATE business
SET num_checkins = (SELECT SUM(total) 
				   	FROM checkins
				   	WHERE checkins.business_id = business.business_id);