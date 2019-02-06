import json


def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def recparsehelp(s, outfile):
    data = json.loads(s)
    for key in data:
        jsonstr = json.dumps(data[key])
        if str(jsonstr)[0] == '{':
            recparsehelp(jsonstr, outfile)
        else:
            outfile.write(str(jsonstr)+'\t')

def parseBusinessData():
    # read the JSON file
    with open('../../JSONfiles/yelp_business.JSON','r') as f:
        outfile =  open('business.txt', 'w')
        line = f.readline()
        count_line = 0
        # read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+'\t')  # business id
            outfile.write(cleanStr4SQL(data['name'])+'\t')  # name
            outfile.write(cleanStr4SQL(data['address'])+'\t')  # full_address
            outfile.write(cleanStr4SQL(data['state'])+'\t')  # state
            outfile.write(cleanStr4SQL(data['city'])+'\t')  # city
            outfile.write(cleanStr4SQL(data['postal_code']) + '\t')  # zipcode
            outfile.write(str(data['latitude'])+'\t')  # latitude
            outfile.write(str(data['longitude'])+'\t')  # longitude
            outfile.write(str(data['stars'])+'\t') # stars
            outfile.write(str(data['review_count'])+'\t')  # reviewcount
            outfile.write(str(data['is_open'])+'\t')  # openstatus
            outfile.write(str([item for item in data['categories']])+'\t')  # category list
            recparsehelp(json.dumps(data['attributes']), outfile)
            recparsehelp(json.dumps(data['hours']), outfile)
            # outfile.write(str([]))   write your own code to process attributes
            # outfile.write(str([]))  # write your own code to process hours
            outfile.write('\n')
            line = f.readline()
            count_line += 1
    print(count_line)
    outfile.close()
    f.close()


def parseUserData():
    # read the JSON file
    with open('../../JSONfiles/yelp_user.JSON','r') as f:
        outfile = open('user.txt', 'w')
        line = f.readline()
        count_line = 0
        while line:
            data = json.loads(line)
            outfile.write(str(data['average_stars'])+'\t')
            outfile.write(str(data['cool']) + '\t')
            outfile.write(str(data['fans']) + '\t')
            outfile.write(str(data['friends']) + '\t')
            outfile.write(str(data['funny']) + '\t')
            outfile.write(str(data['name']) + '\t')
            outfile.write(str(data['review_count']) + '\t')
            outfile.write(str(data['useful']) + '\t')
            outfile.write(str(data['user_id']) + '\t')
            outfile.write(str(data['yelping_since']) + '\t')
            outfile.write('\n')
            line = f.readline()
            count_line += 1
        print(count_line)
        outfile.close()
        f.close()
    # write code to parse yelp_user.JSON


def parseCheckinData():
    # read the JSON file
    with open('../../JSONfiles/yelp_checkin.JSON','r') as f:
        outfile =  open('checkin.txt', 'w')
        line = f.readline()
        count_line = 0
        while line:
            data = json.loads(line)
            recparsehelp(json.dumps(data['time']), outfile)
            outfile.write(str(data['business_id'])+'\t')
            outfile.write('\n')
            line = f.readline()
            count_line += 1
        print(count_line)
        outfile.close()
        f.close()
    # write code to parse yelp_checkin.JSON


def parseReviewData():
    # read the JSON file
    with open('../../JSONfiles/yelp_review.JSON','r') as f:
        outfile =  open('review.txt', 'w')
        line = f.readline()
        count_line = 0
        while line:
            data = json.loads(line)
            outfile.write(str(data['review_id'])+'\t')
            outfile.write(str(data['user_id'])+'\t')
            outfile.write(str(data['business_id'])+'\t')
            outfile.write(str(data['stars'])+'\t')
            outfile.write(str(data['date'])+'\t')
            outfile.write(str(data['text'])+'\t')
            outfile.write(str(data['useful'])+'\t')
            outfile.write(str(data['funny'])+'\t')
            outfile.write(str(data['cool'])+'\t')
            outfile.write('\n')
            line = f.readline()
            count_line += 1
        print(count_line)
        outfile.close()
        f.close()
    # write code to parse yelp_review.JSON


parseBusinessData()
parseUserData()
parseCheckinData()
parseReviewData()
